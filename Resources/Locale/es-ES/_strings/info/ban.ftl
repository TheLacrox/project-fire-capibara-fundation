# Comando ban
cmd-ban-desc = Impone una sanción a una persona
cmd-ban-help = Uso: ban <nombre o ID de usuario> <motivo> [duración en minutos; omitir o usar 0 para que sea permanente]
cmd-ban-player = No se ha podido encontrar a ningún jugador con ese nombre.
cmd-ban-invalid-minutes = ¡{ $minutes } no es una cantidad de minutos válida!
cmd-ban-invalid-severity = ¡{ $severity } no es un nivel de gravedad válido!
cmd-ban-invalid-arguments = Número de argumentos no válido
cmd-ban-hint = <nombre/ID de usuario>
cmd-ban-hint-reason = <motivo>
cmd-ban-hint-duration = [duración]
cmd-ban-hint-severity = [gravedad]

cmd-ban-hint-duration-1 = Permanente
cmd-ban-hint-duration-2 = 1 día
cmd-ban-hint-duration-3 = 3 días
cmd-ban-hint-duration-4 = 1 semana
cmd-ban-hint-duration-5 = 2 semanas
cmd-ban-hint-duration-6 = 1 mes

# Panel de sanciones
cmd-banpanel-desc = Abre el panel de sanciones
cmd-banpanel-help = Uso: banpanel [nombre o GUID de usuario]
cmd-banpanel-server = No se puede usar desde la consola del servidor
cmd-banpanel-player-err = No se ha podido encontrar al jugador indicado

# Comando banlist
cmd-banlist-desc = Enumera las sanciones activas de un usuario.
cmd-banlist-help = Uso: banlist <nombre o ID de usuario>
cmd-banlist-empty = No se han encontrado sanciones activas para { $user }
cmd-banlist-hint = <nombre/ID de usuario>
cmd-banlistF-hint = <nombre/ID de usuario>

cmd-ban_exemption_update-desc = Configura las excepciones de un jugador a determinados tipos de sanción.
cmd-ban_exemption_update-help = Uso: ban_exemption_update <jugador> <indicador> [<indicador> [...]]
    Especifica varios indicadores para conceder varias excepciones.
    Para eliminar todas las excepciones, ejecuta el comando con "None" como único indicador.
cmd-ban_exemption_update-nargs = Se esperaban al menos 2 argumentos
cmd-ban_exemption_update-locate = No se ha podido localizar a «{ $player }».
cmd-ban_exemption_update-invalid-flag = «{ $flag }» no es un indicador válido.
cmd-ban_exemption_update-success = Se han actualizado las excepciones de «{ $player }» ({ $uid }).
cmd-ban_exemption_update-arg-player = <jugador>
cmd-ban_exemption_update-arg-flag = <indicador>

cmd-ban_exemption_get-desc = Muestra las excepciones de sanciones de un jugador.
cmd-ban_exemption_get-help = Uso: ban_exemption_get <jugador>
cmd-ban_exemption_get-nargs = Se esperaba exactamente 1 argumento
cmd-ban_exemption_get-none = El usuario no está exento de ninguna sanción.
cmd-ban_exemption_get-show = El usuario está exento de las siguientes categorías de sanción: { $flags }.
cmd-ban_exemption_get-arg-player = <jugador>

# Panel de sanciones
ban-panel-title = Panel de sanciones
ban-panel-player = Jugador
ban-panel-ip = IP
ban-panel-hwid = HWID
ban-panel-reason = Motivo
ban-panel-last-conn = ¿Usar la IP y el HWID de la última conexión?
ban-panel-submit = Sancionar
ban-panel-confirm = ¿Confirmas la operación?
ban-panel-tabs-basic = Información básica
ban-panel-tabs-reason = Motivo
ban-panel-tabs-players = Lista de jugadores
ban-panel-tabs-role = Información del veto de puesto
ban-panel-no-data = Debes indicar un usuario, una IP o un HWID al que sancionar
ban-panel-invalid-ip = No se ha podido interpretar la dirección IP. Inténtalo de nuevo
ban-panel-select = Selecciona un tipo
ban-panel-server = Sanción del servidor
ban-panel-role = Veto de puesto
ban-panel-minutes = Minutos
ban-panel-hours = Horas
ban-panel-days = Días
ban-panel-weeks = Semanas
ban-panel-months = Meses
ban-panel-years = Años
ban-panel-permanent = Permanente
ban-panel-ip-hwid-tooltip = Déjalo vacío y marca la casilla inferior para usar los datos de la última conexión
ban-panel-severity = Gravedad:
ban-panel-erase = Eliminar los mensajes del chat y al jugador de la ronda
ban-panel-expiry-error = error

# Cadena de registro de la sanción
server-ban-string = { $admin } ha creado una sanción del servidor de gravedad { $severity } para [{ $name }, { $ip }, { $hwid }], que caduca { $expires }. Motivo: { $reason }. Ronda: { $round }
server-ban-string-no-pii = { $admin } ha creado una sanción del servidor de gravedad { $severity } para { $name }, que caduca { $expires }. Motivo: { $reason }. Ronda: { $round }
server-ban-string-never = nunca
server-ban-unknown-round = Desconocida

# Expulsión al imponer la sanción
ban-kick-reason = Se te ha prohibido el acceso al servidor

# Vetos de antagonista
ban-panel-role-selection-antag = Antagonista
ban-panel-role-selection-antag-all-option = Todos
