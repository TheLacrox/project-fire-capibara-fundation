# Comandos
## Retrasar el final de la ronda
cmd-delayroundend-desc = Detiene el temporizador que finaliza la ronda cuando el transbordador de emergencia sale del hiperespacio.
cmd-delayroundend-help = Uso: delayroundend
emergency-shuttle-command-round-desc = Detiene el temporizador que finaliza la ronda cuando el transbordador de emergencia sale del hiperespacio.
emergency-shuttle-command-round-yes = Ronda retrasada.
emergency-shuttle-command-round-no = No se pudo retrasar el final de la ronda.

## Atracar el transbordador de emergencia
cmd-dockemergencyshuttle-desc = Llama al transbordador de emergencia e intenta atracarlo en la estación.
cmd-dockemergencyshuttle-help = Uso: dockemergencyshuttle
emergency-shuttle-command-dock-desc = Llama al transbordador de emergencia e intenta atracarlo en la estación.

## Lanzar el transbordador de emergencia
cmd-launchemergencyshuttle-desc = Lanza el transbordador de emergencia antes de tiempo, si es posible.
cmd-launchemergencyshuttle-help = Uso: launchemergencyshuttle
emergency-shuttle-command-launch-desc = Lanza el transbordador de emergencia antes de tiempo, si es posible.

# Transbordador de emergencia
emergency-shuttle-left = El transbordador de emergencia ha abandonado la estación. Tiempo estimado hasta su llegada al Mando Central: { $transitTime } segundos.
emergency-shuttle-launch-time = El transbordador de emergencia despegará dentro de { $consoleAccumulator } segundos.
emergency-shuttle-docked = El transbordador de emergencia ha atracado en dirección { $direction } respecto a la estación, { $location }. Partirá dentro de { $time } segundos.{ $extended }
emergency-shuttle-good-luck = El transbordador de emergencia no ha podido encontrar la estación. Buena suerte.
emergency-shuttle-nearby = El transbordador de emergencia no ha encontrado un puerto de atraque válido. Ha saltado a una posición en dirección { $direction } respecto a la estación, { $location }. Partirá dentro de { $time } segundos.{ $extended }
emergency-shuttle-extended = { " " }La hora de despegue se ha retrasado debido a circunstancias adversas.

# Avisos y mensajes de la consola del transbordador
emergency-shuttle-console-no-early-launches = El despegue anticipado está desactivado
emergency-shuttle-console-auth-left =
    { $remaining ->
        [one] Hace falta { $remaining } autorización más para adelantar el despegue.
       *[other] Hacen falta { $remaining } autorizaciones más para adelantar el despegue.
    }
emergency-shuttle-console-auth-revoked =
    Se ha revocado la autorización de despegue anticipado. { $remaining ->
        [one] Hace falta { $remaining } autorización.
       *[other] Hacen falta { $remaining } autorizaciones.
    }
emergency-shuttle-console-denied = Acceso denegado

# Interfaz
emergency-shuttle-console-window-title = Consola del transbordador de emergencia
emergency-shuttle-ui-engines = MOTORES:
emergency-shuttle-ui-idle = Inactivos
emergency-shuttle-ui-repeal-all = REVOCAR TODO
emergency-shuttle-ui-early-authorize = Autorización de despegue anticipado
emergency-shuttle-ui-authorize = AUTORIZAR
emergency-shuttle-ui-repeal = REVOCAR
emergency-shuttle-ui-authorizations = Autorizaciones
emergency-shuttle-ui-remaining = Restantes: { $remaining }

# Mapas
map-name-centcomm = Mando Central
map-name-terminal = Terminal de llegadas
