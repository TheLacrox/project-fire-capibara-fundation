zombie-title = Zombis
zombie-description = ¡Los no muertos andan sueltos por la estación! Colabora con la tripulación para sobrevivir al brote y asegurar la estación.

zombieteors-title = Zombiteoritos
zombieteors-description = ¡Los no muertos andan sueltos por la estación en plena lluvia de meteoritos catastrófica! ¡Colabora con tus compañeros y haz lo que puedas por sobrevivir!

zombie-not-enough-ready-players = ¡No hay suficientes jugadores listos para la partida! Había { $readyPlayersCount } jugadores listos de los { $minimumPlayers } necesarios. No se puede iniciar Zombis.
zombie-no-one-ready = ¡No hay ningún jugador listo! No se puede iniciar Zombis.

zombie-patientzero-role-greeting = Eres uno de los infectados iniciales. Consigue suministros y prepárate para tu transformación. Tu objetivo es hacerte con la estación infectando a cuanta más gente mejor.
zombie-healing = Notas que algo se remueve en tu carne
zombie-infection-warning = Notas que el virus zombi se hace fuerte
zombie-infection-underway = Tu sangre empieza a espesarse

zombie-alone = Te sientes completamente solo.

zombie-shuttle-call = Hemos detectado que los no muertos se han hecho con la estación. Enviamos una lanzadera de emergencia para recoger al personal restante.

zombie-round-end-initial-count = { $initialCount ->
    [one] Hubo un infectado inicial:
    *[other] Hubo { $initialCount } infectados iniciales:
}
zombie-round-end-user-was-initial = - [color=plum]{ $name }[/color] ([color=gray]{ $username }[/color]) fue uno de los infectados iniciales.

zombie-round-end-amount-none = [color=green]¡Se erradicó a todos los zombis![/color]
zombie-round-end-amount-low = [color=green]Se exterminó a casi todos los zombis.[/color]
zombie-round-end-amount-medium = [color=yellow]El { $percent } % de la tripulación se convirtió en zombi.[/color]
zombie-round-end-amount-high = [color=crimson]El { $percent } % de la tripulación se convirtió en zombi.[/color]
zombie-round-end-amount-all = [color=darkred]¡Toda la tripulación se convirtió en zombi![/color]

zombie-round-end-survivor-count = { $count ->
    [one] Solo quedó un superviviente:
    *[other] Solo quedaron { $count } supervivientes:
}
zombie-round-end-user-was-survivor = - [color=White]{ $name }[/color] ([color=gray]{ $username }[/color]) sobrevivió al brote.
