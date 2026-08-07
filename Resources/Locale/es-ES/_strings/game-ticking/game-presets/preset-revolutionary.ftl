## Rev Head

roles-antag-rev-head-name = Líder revolucionario
roles-antag-rev-head-objective = Tu objetivo es hacerte con la estación convirtiendo gente a tu causa y eliminando a todos los miembros del Mando.

head-rev-role-greeting =
    Eres un líder revolucionario. Tu misión es apartar del poder a todo el Mando, ya sea matándolos, inmovilizándolos o convirtiéndolos.
    El Sindicato te ha patrocinado con un flash que convierte a los demás a tu causa. Cuidado: no funciona con quien lleve protección ocular ni implante de escudo mental. Recuerda que al Mando y a Seguridad les implantan escudos mentales durante la contratación.
    ¡Viva la revolución!

head-rev-briefing =
    Usa flashes para convertir gente a tu causa.
    Mata, inmoviliza o convierte a todos los miembros del Mando para hacerte con la estación.

head-rev-break-mindshield = ¡El implante de escudo mental ha sido destruido!

## Rev

roles-antag-rev-name = Revolucionario
roles-antag-rev-objective = Tu objetivo es velar por la seguridad de los líderes revolucionarios, seguir sus órdenes y ayudarles a tomar la estación eliminando a todos los miembros del Mando.

rev-break-control = ¡{ $name } ha recordado dónde está su verdadera lealtad!

rev-role-greeting =
    Eres un revolucionario. Tu misión es proteger a los líderes revolucionarios y ayudarles a hacerse con la estación.
    La revolución debe actuar unida para matar, inmovilizar o convertir a todos los miembros del Mando.
    ¡Viva la revolución!

rev-briefing = Ayuda a los líderes revolucionarios a matar, inmovilizar o convertir a todos los miembros del Mando para hacerse con la estación.

## General

rev-title = Revolucionarios
rev-description = Unos revolucionarios ocultos entre la tripulación buscan convertir a otros a su causa y derrocar al Mando.

rev-not-enough-ready-players = No hay suficientes jugadores listos para la partida. Había { $readyPlayersCount } jugadores listos de los { $minimumPlayers } necesarios. No se puede iniciar Revolucionarios.
rev-no-one-ready = ¡No hay ningún jugador listo! No se puede iniciar Revolucionarios.
rev-no-heads = No se ha podido seleccionar a ningún líder revolucionario. No se puede iniciar Revolucionarios.

rev-won = Los líderes revolucionarios sobrevivieron y lograron hacerse con el control de la estación.

rev-lost = Todos los líderes revolucionarios murieron y el Mando sobrevivió.

rev-stalemate = Tanto el Mando como los líderes revolucionarios murieron. Es un empate.

rev-reverse-stalemate = Tanto el Mando como los líderes revolucionarios sobrevivieron.

rev-headrev-count = { $initialCount ->
    [one] Hubo un líder revolucionario:
    *[other] Hubo { $initialCount } líderes revolucionarios:
}

rev-headrev-name-user = [color=#5e9cff]{ $name }[/color] ([color=gray]{ $username }[/color]) convirtió a { $count } { $count ->
    [one] persona
    *[other] personas
}

rev-headrev-name = [color=#5e9cff]{ $name }[/color] convirtió a { $count } { $count ->
    [one] persona
    *[other] personas
}

## Deconverted window

rev-deconverted-title = ¡Desconvertido!
rev-deconverted-text =
    Con la muerte del último líder revolucionario, la revolución ha terminado.

    Ya no eres revolucionario, así que pórtate bien.
rev-deconverted-confirm = Confirmar
