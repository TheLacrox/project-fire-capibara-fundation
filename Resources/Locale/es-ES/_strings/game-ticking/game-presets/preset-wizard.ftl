## Survivor

roles-antag-survivor-name = Superviviente
# It's a Halo reference
roles-antag-survivor-objective = Objetivo actual: sobrevivir

survivor-role-greeting =
    Eres un superviviente. Por encima de todo, tienes que volver con vida al Mando Central.
    Reúne toda la potencia de fuego que haga falta para garantizar tu supervivencia.
    No te fíes de nadie.

survivor-round-end-dead-count =
{
    $deadCount ->
        [one] Murió [color=red]{ $deadCount }[/color] superviviente.
        *[other] Murieron [color=red]{ $deadCount }[/color] supervivientes.
}

survivor-round-end-alive-count =
{
    $aliveCount ->
        [one] [color=yellow]{ $aliveCount }[/color] superviviente se quedó abandonado en la estación.
        *[other] [color=yellow]{ $aliveCount }[/color] supervivientes se quedaron abandonados en la estación.
}

survivor-round-end-alive-on-shuttle-count =
{
    $aliveCount ->
        [one] [color=green]{ $aliveCount }[/color] superviviente salió con vida.
        *[other] [color=green]{ $aliveCount }[/color] supervivientes salieron con vida.
}

## Wizard

objective-issuer-swf = [color=turquoise]La Federación de Magos Espaciales[/color]

wizard-title = Mago
wizard-description = ¡Hay un mago en la estación! Nunca se sabe lo que puede hacer.

roles-antag-wizard-name = Mago
roles-antag-wizard-objective = Dales una lección que no olvidarán jamás.

wizard-role-greeting =
    ¡Es la hora de la magia, bola de fuego!
    Ha habido tensiones entre la Federación de Magos Espaciales y NanoTrasen. La Federación te ha elegido para visitar la estación y «recordarles» por qué no conviene jugar con los hechiceros.
    ¡Siembra el caos y la destrucción! Lo que hagas es cosa tuya, pero recuerda que los Magos Espaciales quieren que salgas de allí con vida.

wizard-round-end-name = mago

## TODO: Wizard Apprentice (Coming sometime post-wizard release)
