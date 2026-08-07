### Comando roleban
cmd-roleban-desc = Impide que un jugador desempeñe un rol
cmd-roleban-help = Uso: roleban <nombre o ID de usuario> <puesto> <motivo> [duración en minutos; omitir o usar 0 para que sea permanente]

## Sugerencias de autocompletado
cmd-roleban-hint-1 = <nombre o ID de usuario>
cmd-roleban-hint-2 = <puesto>
cmd-roleban-hint-3 = <motivo>
cmd-roleban-hint-4 = [duración en minutos; omitir o usar 0 para que sea permanente]
cmd-roleban-hint-5 = [gravedad]

cmd-roleban-hint-duration-1 = Permanente
cmd-roleban-hint-duration-2 = 1 día
cmd-roleban-hint-duration-3 = 3 días
cmd-roleban-hint-duration-4 = 1 semana
cmd-roleban-hint-duration-5 = 2 semanas
cmd-roleban-hint-duration-6 = 1 mes

### Comando roleunban
cmd-roleunban-desc = Retira el veto de rol de un jugador
cmd-roleunban-help = Uso: roleunban <ID del veto de rol>
cmd-roleunban-unable-to-parse-id = No se ha podido interpretar { $id } como un ID numérico de veto.
    { $help }

## Sugerencias de autocompletado
cmd-roleunban-hint-1 = <ID del veto de rol>

### Comando rolebanlist
cmd-rolebanlist-desc = Enumera los vetos de rol de un usuario
cmd-rolebanlist-help = Uso: rolebanlist <nombre o ID de usuario> [incluir vetos retirados]

## Sugerencias de autocompletado
cmd-rolebanlist-hint-1 = <nombre o ID de usuario>
cmd-rolebanlist-hint-2 = [incluir vetos retirados]

cmd-roleban-minutes-parse = { $time } no es una cantidad de minutos válida.\n{ $help }
cmd-roleban-severity-parse = «{ $severity }» no es un nivel de gravedad válido.\n{ $help }
cmd-roleban-arg-count = Número de argumentos no válido.
cmd-roleban-job-parse = El puesto { $job } no existe.
cmd-roleban-name-parse = No se ha podido encontrar a ningún jugador con ese nombre.
cmd-roleban-existing = { $target } ya tiene vetado el rol { $role }.
cmd-roleban-success = Se ha vetado a { $target } del rol { $role } { $length }. Motivo: { $reason }.
cmd-roleban-inf = permanentemente
cmd-roleban-until = hasta { $expires }

# Vetos de departamento
cmd-departmentban-desc = Impide que un jugador desempeñe los puestos de un departamento
cmd-departmentban-help = Uso: departmentban <nombre o ID de usuario> <departamento> <motivo> [duración en minutos; omitir o usar 0 para que sea permanente]
