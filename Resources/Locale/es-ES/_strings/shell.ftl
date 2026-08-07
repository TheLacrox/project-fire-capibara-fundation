### Mensajes técnicos y del sistema

## General

shell-command-success = Comando ejecutado correctamente.
shell-invalid-command = Comando no válido.
shell-invalid-command-specific = El comando { $commandName } no es válido.
shell-can-only-run-from-pre-round-lobby = Solo puedes ejecutar este comando desde la sala de espera anterior a la ronda.
shell-can-only-run-while-round-is-active = Solo puedes ejecutar este comando durante una ronda.
shell-cannot-run-command-from-server = No puedes ejecutar este comando desde el servidor.
shell-only-players-can-run-this-command = Solo los jugadores pueden ejecutar este comando.
shell-must-be-attached-to-entity = Debes estar vinculado a una entidad para ejecutar este comando.
shell-must-have-body = Debes tener un cuerpo para ejecutar este comando.

## Argumentos

shell-need-exactly-one-argument = Se necesita exactamente un argumento.
shell-wrong-arguments-number-need-specific =
    Se necesitan { $properAmount } { $properAmount ->
        [one] argumento
       *[other] argumentos
    }, pero se han recibido { $currentAmount }.
shell-argument-must-be-number = El argumento debe ser un número.
shell-argument-must-be-boolean = El argumento debe ser un valor booleano.
shell-wrong-arguments-number = Número de argumentos incorrecto.
shell-need-between-arguments = ¡Se necesitan entre { $lower } y { $upper } argumentos!
shell-need-minimum-arguments = ¡Se necesitan al menos { $minimum } argumentos!
shell-need-minimum-one-argument = ¡Se necesita al menos un argumento!
shell-need-exactly-zero-arguments = Este comando no admite argumentos.

shell-argument-uid = EntityUid

## Comprobaciones

shell-missing-required-permission = ¡Necesitas el permiso { $perm } para utilizar este comando!
shell-entity-is-not-mob = ¡La entidad de destino no es un ser vivo!
shell-invalid-entity-id = ID de entidad no válido.
shell-invalid-grid-id = ID de cuadrícula no válido.
shell-invalid-map-id = ID de mapa no válido.
shell-invalid-entity-uid = { $uid } no es un UID de entidad válido.
shell-invalid-bool = Valor booleano no válido.
shell-entity-uid-must-be-number = EntityUid debe ser un número.
shell-could-not-find-entity = No se ha podido encontrar la entidad { $entity }.
shell-could-not-find-entity-with-uid = No se ha podido encontrar ninguna entidad con el UID { $uid }.
shell-entity-with-uid-lacks-component = La entidad con el UID { $uid } no tiene el componente { $componentName }.
shell-entity-target-lacks-component = La entidad de destino no tiene el componente { $componentName }.
shell-invalid-color-hex = ¡Color hexadecimal no válido!
shell-target-player-does-not-exist = ¡El jugador de destino no existe!
shell-target-entity-does-not-have-message = ¡La entidad de destino no tiene { $missing }!
shell-timespan-minutes-must-be-correct = { $span } no es un intervalo de minutos válido.
shell-argument-must-be-prototype = ¡El argumento { $index } debe ser un prototipo de tipo { LOC($prototypeName) }!
shell-argument-number-must-be-between = ¡El argumento { $index } debe ser un número entre { $lower } y { $upper }!
shell-argument-station-id-invalid = ¡El argumento { $index } debe ser un ID de estación válido!
shell-argument-map-id-invalid = ¡El argumento { $index } debe ser un ID de mapa válido!
shell-argument-number-invalid = ¡El argumento { $index } debe ser un número válido!

# Sugerencias
shell-argument-username-hint = <nombre de usuario>
shell-argument-username-optional-hint = [nombre de usuario]
