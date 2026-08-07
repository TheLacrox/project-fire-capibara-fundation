### Comandos de consola del sistema de votación

## Comando `createvote`
cmd-createvote-desc = Crea una votación
cmd-createvote-help = Uso: createvote <'restart'|'preset'|'map'>
cmd-createvote-cannot-call-vote-now = ¡No puedes iniciar una votación ahora mismo!
cmd-createvote-invalid-vote-type = Tipo de votación no válido
cmd-createvote-arg-vote-type = <tipo de votación>

## Comando `customvote`
cmd-customvote-desc = Crea una votación personalizada
cmd-customvote-help = Uso: customvote <título> <opción1> <opción2> [opción3...]
cmd-customvote-on-finished-tie = La votación «{ $title }» ha terminado con un empate entre { $ties }.
cmd-customvote-on-finished-win = La votación «{ $title }» ha terminado: ha ganado { $winner }.
cmd-customvote-arg-title = <título>
cmd-customvote-arg-option-n = <opción{ $n }>

## Comando `vote`
cmd-vote-desc = Vota en una votación activa
cmd-vote-help = Uso: vote <voteId> <opción>
cmd-vote-cannot-call-vote-now = ¡No puedes votar ahora mismo!
cmd-vote-on-execute-error-must-be-player = Debes ser un jugador
cmd-vote-on-execute-error-invalid-vote-id = ID de votación no válido
cmd-vote-on-execute-error-invalid-vote-options = Opciones de votación no válidas
cmd-vote-on-execute-error-invalid-vote = Votación no válida
cmd-vote-on-execute-error-invalid-option = Opción no válida

## Comando `listvotes`
cmd-listvotes-desc = Enumera las votaciones activas
cmd-listvotes-help = Uso: listvotes

## Comando `cancelvote`
cmd-cancelvote-desc = Cancela una votación activa
cmd-cancelvote-help =
    Uso: cancelvote <id>
    Puedes obtener el ID mediante el comando listvotes.
cmd-cancelvote-error-invalid-vote-id = ID de votación no válido
cmd-cancelvote-error-missing-vote-id = Falta el ID
cmd-cancelvote-arg-id = <id>
