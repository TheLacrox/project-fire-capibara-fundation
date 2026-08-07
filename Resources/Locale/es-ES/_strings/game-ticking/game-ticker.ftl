game-ticker-restart-round = Reiniciando la ronda...
game-ticker-start-round = La ronda comienza ahora...
game-ticker-start-round-cannot-start-game-mode-fallback = No se pudo iniciar el modo { $failedGameMode }. Se usará { $fallbackMode }...
game-ticker-start-round-cannot-start-game-mode-restart = No se pudo iniciar el modo { $failedGameMode }. Reiniciando la ronda...
game-ticker-start-round-invalid-map = El mapa seleccionado, { $map }, no es apto para el modo { $mode }. Es posible que el modo no funcione como debería...
game-ticker-unknown-role = Desconocido
game-ticker-delay-start =
    El inicio de la ronda se ha retrasado { $seconds ->
        [one] { $seconds } segundo.
       *[other] { $seconds } segundos.
    }
game-ticker-pause-start = Se ha pausado el inicio de la ronda.
game-ticker-pause-start-resumed = Se ha reanudado la cuenta atrás para el inicio de la ronda.
game-ticker-player-join-game-message = ¡Bienvenido a la Fundación SCP de Project Fire! Si es tu primera partida, abre el menú de pausa y lee las reglas. No tengas miedo de pedir ayuda mediante la ayuda administrativa o por LOOC/OOC cuando estén disponibles.
game-ticker-get-info-text = Bienvenido a [color=white]la Fundación SCP de Project Fire.[/color]
                            Ronda actual: [color=white]#{ $roundId }[/color]
                            Jugadores conectados: [color=white]{ $playerCount }[/color]
                            Mapa actual: [color=white]{ $mapName }[/color]
                            Modo de juego actual: [color=white]{ $gmTitle }[/color]
                            >[color=yellow]{ $desc }[/color]
game-ticker-get-info-preround-text = Bienvenido a [color=white]la Fundación SCP de Project Fire.[/color]
                            Ronda actual: [color=white]#{ $roundId }[/color]
                            Jugadores conectados: [color=white]{ $playerCount }[/color] ([color=white]{ $readyCount }[/color] { $readyCount ->
                                [one] ha confirmado que está listo
                               *[other] han confirmado que están listos
                            })
                            Mapa actual: [color=white]{ $mapName }[/color]
                            Modo de juego actual: [color=white]{ $gmTitle }[/color]
                            >[color=yellow]{ $desc }[/color]
game-ticker-no-map-selected = [color=yellow]¡Todavía no se ha elegido un mapa![/color]
game-ticker-player-no-jobs-available-when-joining = No había ningún puesto disponible al intentar entrar en la partida.

# Mostrado a los administradores cuando entra un jugador
player-join-message = El jugador { $name } se ha conectado.
player-first-join-message = El jugador { $name } se ha conectado por primera vez.

# Mostrado a los administradores cuando sale un jugador
player-leave-message = El jugador { $name } se ha desconectado.

latejoin-arrival-announcement = ¡{ $character } ({ $job }) ha llegado al complejo!
latejoin-arrival-announcement-special = ¡{ $job } { $character } ya está en el complejo!
latejoin-arrival-sender = Complejo
latejoin-arrivals-direction = En breve llegará un transbordador que te llevará a tu complejo.
latejoin-arrivals-direction-time = El transbordador que te llevará a tu complejo llegará dentro de { $time }.
latejoin-arrivals-dumped-from-shuttle = Una fuerza misteriosa te impide marcharte en el transbordador de llegadas.
latejoin-arrivals-teleport-to-spawn = Una fuerza misteriosa te teletransporta fuera del transbordador de llegadas. ¡Que tengas un turno seguro!

preset-not-enough-ready-players = No se puede iniciar { $presetName }. Se necesitan { $minimumPlayers } jugadores, pero solo { $readyPlayersCount } han confirmado que están listos.
preset-not-enough-ready-command-staff = No se puede iniciar { $presetName }. Se necesitan { $minimumCommandStaff } miembros del personal de mando, pero solo hay { $readyCommandStaffCount } disponibles.
preset-no-one-ready = No se puede iniciar { $presetName }. Ningún jugador ha confirmado que esté listo.

game-run-level-PreRoundLobby = Vestíbulo previo a la ronda
game-run-level-InRound = Ronda en curso
game-run-level-PostRound = Fin de la ronda
