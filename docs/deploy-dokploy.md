# Despliegue de Fundación Capibara en Dokploy

Servidor de SCP: Project Fire (fork Capibara Foundation) empaquetado como imagen Docker
y desplegado con Dokploy.

## Servicios

Un único servicio: `game-server`.

A diferencia de Estación Capibara, aquí **no hay `redis` ni `tts-worker`**. El TTS de este
fork habla con una API HTTP externa (`tts.api_url` + `tts.api_token`), no con una cola en
redis, así que el stack se queda en un solo contenedor.

## Cómo se construye

`Dockerfile` en dos etapas:

1. **build** — `mcr.microsoft.com/dotnet/sdk:10.0`. Clona el submódulo `RobustToolbox`
   (público, sin auth), restaura, compila `Content.Packaging` en Release y empaqueta un
   servidor `linux-x64` con el cliente embebido (`--hybrid-acz`), de modo que el launcher
   se descargue el cliente solo. Produce `release/SS14.Server_linux-x64.zip`.
2. **runtime** — `mcr.microsoft.com/dotnet/runtime:10.0`. El servidor es
   *framework-dependent*, así que necesita la imagen de runtime. Corre como usuario
   `ss14` (uid 10001), nunca como root.

El árbol privado `SunrisePrivate/` no está en la imagen, y es correcto: todos los
`Content.*.csproj` lo condicionan con `Exists(...)`, así que su ausencia es una
configuración de compilación soportada y no define `SUNRISE_PRIVATE`.

## Puertos

| Puerto | Protocolo | Para qué | ¿Proxy? |
|---|---|---|---|
| 1212 | UDP | Juego | **No.** Traefik no hace proxy de UDP. Tiene que ser puerto directo del host. |
| 1212 | TCP | Status/launcher | Sí. Puede ir por Dokploy/Traefik con HTTPS. |

## Configuración en Dokploy

1. Crea una aplicación de tipo **Compose** apuntando a este repositorio.
   - Rama: `main`
   - Ruta del compose: `docker-compose.yml` (está en la raíz)
   - Dokploy construye desde el código; no hace falta publicar la imagen en ningún registro.
2. Activa **auto-deploy** (webhook de push) si quieres que cada push a `main` redespliegue.
3. Activa **Enable Isolated Deployment** (ver más abajo).
4. **Submódulos**: no hace falta que Dokploy los clone. El `Dockerfile` ejecuta
   `git submodule update --init --recursive` por su cuenta y `RobustToolbox` es un
   repositorio público sin auth. Solo hace falta que `.git` esté en el contexto de
   construcción, que es lo que hace un checkout normal de Dokploy.
5. Define las variables de entorno (sección siguiente).
6. **Red**:
   - **UDP 1212** tiene que estar abierto en el host y publicado. Traefik no hace proxy de
     UDP, así que este puerto va directo.
   - **TCP 1212** es el endpoint de status/launcher. Ponlo detrás del dominio de Dokploy con
     HTTPS (Traefik) para que el launcher use `ss14s://<dominio>`, o exponlo directamente.
7. **Recursos del host**: la construcción desde código es pesada. En una máquina de 10 CPUs
   tardó unos 10 minutos y la imagen final ocupa 2,3 GB, más el contexto de ~3,3 GB. Da
   CPU, RAM y disco suficientes al host de Dokploy.

## Aislamiento de red (Enable Isolated Deployment)

Activa el interruptor **Enable Isolated Deployment** de Dokploy para esta aplicación. Crea
una red Docker propia por aplicación, separada de `dokploy-network` y del resto de apps,
manteniendo la salida a internet que el servidor necesita para el hub y la autenticación.

Por eso `docker-compose.yml` **no define ningún bloque `networks:`** ni referencia a
`dokploy-network`: del aislamiento se encarga Dokploy. No añadas un `networks:` propio,
porque pelea con la inyección de red de Dokploy y rompe la resolución de nombres.

### Variables disponibles

| Variable | Efecto | Por defecto |
|---|---|---|
| `SS14_DOMAIN` | Fija `hub.server_url=ss14s://<dominio>` y `status.connectaddress=udp://<dominio>:1212` | vacío |
| `SS14_HOSTNAME` | Nombre en el navegador de servidores | `[ES] Fundación Capibara [Español] [SCP]` |
| `SS14_DESC` | Descripción en el navegador | la del TOML |
| `SS14_HUB_ADVERTISE` | Anunciarse en el hub público | `true` |
| `SS14_AUTH_MODE` | `0` opcional, `1` requerido, `2` desactivado | `1` |
| `SS14_HOST_USER` | Cuenta que recibe host completo al entrar | `TheLacrox` |
| `SS14_SOFT_MAX` | Límite blando de jugadores | `50` |
| `SS14_DISCORD` / `SS14_WEBSITE` / `SS14_WIKI` | Botones de info del launcher | Discord, web y wiki de Estación Capibara |
| `SS14_TTS_ENABLED` | Activar TTS | `false` |
| `SS14_TTS_API_URL` | Endpoint de la API de TTS | vacío |
| `SS14_TTS_API_TOKEN` | Token de la API de TTS (**secreto**) | vacío |

`SS14_DOMAIN` es la única que conviene fijar siempre: sin ella el launcher no sabe a qué
dirección mandar a los jugadores.

### Lo mínimo para desplegar

Pegado tal cual en el panel de variables de Dokploy:

```
SS14_DOMAIN=tu-dominio.com
```

Eso es todo. El resto tiene valores horneados en `Docker/server_config.prod.toml` que ya
son los correctos para producción.

### Si además quieres TTS

```
SS14_TTS_ENABLED=true
SS14_TTS_API_URL=https://tu-api-de-tts/...
SS14_TTS_API_TOKEN=el-token
```

El token es confidencial: va aquí y nunca en el TOML, porque el TOML se hornea en la imagen.
El `entrypoint.sh` no imprime la lista de argumentos precisamente para que el token no acabe
en los logs del contenedor.

### Ejemplo completo con todo explícito

```
SS14_DOMAIN=tu-dominio.com
SS14_HOSTNAME=[ES] Fundación Capibara [Español] [SCP]
SS14_HUB_ADVERTISE=true
SS14_AUTH_MODE=1
SS14_HOST_USER=TheLacrox
SS14_SOFT_MAX=50
SS14_TTS_ENABLED=false
```

## Seguridad

- `console.loginlocal=false`, forzado tanto en el TOML como por `--cvar` en el entrypoint.
  Detrás de un proxy inverso **todas las conexiones parecen loopback**, así que dejarlo
  activado daría host completo a cualquiera capaz de alcanzar el proxy.
- `auth.mode=1` (requerido). `console.login_host_user` solo es seguro con este modo,
  porque ata el nombre a una cuenta real de SS14.
- `tts.api_token` va únicamente por entorno. El entrypoint **no imprime** la lista de
  argumentos precisamente para no filtrarlo a los logs.
- El contenedor corre como usuario sin privilegios.

## Persistencia

El volumen `ss14-data` se monta en `/data` y es el `--data-dir` del servidor. Contiene
`preferences.db` (SQLite, personajes y ajustes de los jugadores) y los logs. **No lo
borres entre despliegues**: perderías todos los personajes.

## fork_id

`build.fork_id = "capibara-fundacion"`, distinto a propósito del `"capibara"` que usa
Estación Capibara. El launcher cachea los ZIP del cliente por `fork_id`; si dos forks
distintos compartieran el mismo, a quien juegue en ambos le serviría el cliente
equivocado.

## Comprobaciones tras desplegar

1. El contenedor levanta y el log muestra `Server Version ... -> Ready`.
2. `Socket bound to 0.0.0.0:1212` aparece en el log.
3. El servidor sale en el navegador del launcher con el nombre correcto.
4. Un cliente conecta a `udp://<dominio>:1212` y llega al lobby.
5. Reinicia el contenedor y comprueba que los personajes siguen ahí (valida el volumen).

## Prueba local

Requiere Docker en marcha:

```bash
docker compose build
docker compose up
```

Luego conecta un cliente a `udp://localhost:1212`. Para probar solo la construcción:

```bash
docker build -t fundacion-capibara .
```

La compilación es larga: restaura y compila el contenido completo de SS14 más el
empaquetado del cliente.
