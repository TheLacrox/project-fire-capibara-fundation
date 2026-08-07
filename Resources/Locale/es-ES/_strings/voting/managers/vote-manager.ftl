# Iniciador mostrado cuando ningún usuario crea la votación
ui-vote-initiator-server = El servidor

## Votaciones predeterminadas
ui-vote-restart-title = Reiniciar la ronda
ui-vote-restart-succeeded = La votación para reiniciar la ronda ha sido aprobada.
ui-vote-restart-failed = La votación para reiniciar la ronda ha sido rechazada; se necesita un { TOSTRING($ratio, "P0") }.
ui-vote-restart-fail-not-enough-ghost-players = No se pudo iniciar la votación: al menos un { $ghostPlayerRequirement } % de los jugadores debe ser fantasma y ahora mismo no hay suficientes.
ui-vote-restart-yes = Sí
ui-vote-restart-no = No
ui-vote-restart-abstain = Abstenerse

ui-vote-gamemode-title = Próximo modo de juego
ui-vote-gamemode-tie = ¡Empate en la votación del modo de juego! Se ha elegido: { $picked }
ui-vote-gamemode-win = ¡{ $winner } ha ganado la votación del modo de juego!
ui-vote-gamemode-auto-set = Solo hay un modo disponible: { $preset }. Se ha omitido la votación.

ui-vote-map-title = Próximo mapa
ui-vote-map-tie = ¡Empate en la votación del mapa! Se ha elegido: { $picked }
ui-vote-map-win = ¡{ $winner } ha ganado la votación del mapa!
ui-vote-map-notlobby = ¡Solo se puede votar el mapa siguiente en el vestíbulo previo a la ronda!
ui-vote-map-notlobby-time = ¡Solo se puede votar el mapa siguiente en el vestíbulo previo a la ronda cuando quedan { $time }!
ui-vote-secret-map = Secreto
ui-vote-secret-win = El próximo mapa se elegirá al azar.

# Votaciones de expulsión
ui-vote-votekick-unknown-initiator = Un jugador
ui-vote-votekick-unknown-target = Jugador desconocido
ui-vote-votekick-title = { $initiator } ha iniciado una votación para expulsar a { $targetEntity }. Motivo: { $reason }
ui-vote-votekick-yes = Sí
ui-vote-votekick-no = No
ui-vote-votekick-abstain = Abstenerse
ui-vote-votekick-success = Se ha aprobado la expulsión de { $target }. Motivo: { $reason }
ui-vote-votekick-failure = Se ha rechazado la expulsión de { $target }. Motivo: { $reason }
ui-vote-votekick-not-enough-eligible = No hay suficientes votantes aptos en línea para iniciar una expulsión: { $voters }/{ $requirement }
ui-vote-votekick-server-cancelled = El servidor ha cancelado la votación para expulsar a { $target }.
