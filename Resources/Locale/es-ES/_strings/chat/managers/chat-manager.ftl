### Interfaz

chat-manager-max-message-length = Tu mensaje supera el límite de { $maxMessageLength } caracteres.
chat-manager-ooc-chat-enabled-message = Se ha habilitado el chat OOC.
chat-manager-ooc-chat-disabled-message = Se ha deshabilitado el chat OOC.
chat-manager-looc-chat-enabled-message = Se ha habilitado el chat LOOC.
chat-manager-looc-chat-disabled-message = Se ha deshabilitado el chat LOOC.
chat-manager-dead-looc-chat-enabled-message = Los jugadores muertos ya pueden usar LOOC.
chat-manager-dead-looc-chat-disabled-message = Los jugadores muertos ya no pueden usar LOOC.
chat-manager-crit-looc-chat-enabled-message = Los jugadores en estado crítico ya pueden usar LOOC.
chat-manager-crit-looc-chat-disabled-message = Los jugadores en estado crítico ya no pueden usar LOOC.
chat-manager-admin-ooc-chat-enabled-message = Se ha habilitado el chat OOC de administración.
chat-manager-admin-ooc-chat-disabled-message = Se ha deshabilitado el chat OOC de administración.

chat-manager-max-message-length-exceeded-message = Tu mensaje ha superado el límite de { $limit } caracteres.
chat-manager-no-headset-on-message = ¡No llevas auriculares!
chat-manager-no-radio-key = ¡No has especificado ninguna clave de radio!
chat-manager-no-such-channel = ¡No existe ningún canal con la clave «{ $key }»!
chat-manager-whisper-headset-on-message = ¡No puedes susurrar por la radio!

chat-manager-server-wrap-message = [bold]{ $message }[/bold]
chat-manager-sender-announcement = Mando Central
chat-manager-sender-announcement-wrap-message = [font size=14][bold]Anuncio de { $sender }:[/font][font size=12]
    { $message }[/bold][/font]
chat-manager-entity-say-wrap-message = [BubbleHeader][bold][Name]{ $entityName }[/Name][/bold][/BubbleHeader] { $verb }: [font={$fontType} size={$fontSize}]«[BubbleContent]{ $message }[/BubbleContent]»[/font]
chat-manager-entity-say-bold-wrap-message = [BubbleHeader][bold][Name]{ $entityName }[/Name][/bold][/BubbleHeader] { $verb }: [font={$fontType} size={$fontSize}]«[BubbleContent][bold]{ $message }[/bold][/BubbleContent]»[/font]

chat-manager-entity-whisper-wrap-message = [font size=11][italic][BubbleHeader][Name]{ $entityName }[/Name][/BubbleHeader] susurra: «[BubbleContent]{ $message }[/BubbleContent]»[/italic][/font]
chat-manager-entity-whisper-unknown-wrap-message = [font size=11][italic][BubbleHeader]Alguien[/BubbleHeader] susurra: «[BubbleContent]{ $message }[/BubbleContent]»[/italic][/font]

# THE() no se usa aquí porque la entidad y su nombre pueden no coincidir si se proporciona nameOverride.
chat-manager-entity-me-wrap-message = [italic]{ PROPER($entity) ->
    *[false] { $entityName } { $message }[/italic]
     [true] { CAPITALIZE($entityName) } { $message }[/italic]
    }

chat-manager-entity-looc-wrap-message = LOOC: [bold]{ $entityName }:[/bold] { $message }
chat-manager-send-ooc-wrap-message = OOC: [bold]{ $playerName }:[/bold] { $message }
chat-manager-send-ooc-sponsor-wrap-message = OOC: [bold][color={$sponsorColor}]{ $playerName }[/color]:[/bold] { $message }
chat-manager-send-ooc-patron-wrap-message = OOC: [bold][color={$patronColor}]{ $playerName }[/color]:[/bold] { $message }
chat-manager-send-ooc-admin-wrap-message = OOC: [bold]{ $patronTitle }{ $playerName }:[/bold] { $message }

chat-manager-send-dead-chat-wrap-message = { $deadChannelName }: [bold][BubbleHeader]{ $playerName }[/BubbleHeader]:[/bold] [BubbleContent]{ $message }[/BubbleContent]
chat-manager-send-admin-dead-chat-wrap-message = { $adminChannelName }: [bold]([BubbleHeader]{ $userName }[/BubbleHeader]):[/bold] [BubbleContent]{ $message }[/BubbleContent]
chat-manager-send-admin-chat-wrap-message = { $adminChannelName }: [bold]{ $playerName }:[/bold] { $message }
chat-manager-send-admin-announcement-wrap-message = [bold]{ $adminChannelName }: { $message }[/bold]

chat-manager-send-hook-ooc-wrap-message = OOC: [bold](D){ $senderName }:[/bold] { $message }
chat-manager-send-hook-admin-wrap-message = ADMINISTRACIÓN: [bold](D){ $senderName }:[/bold] { $message }

chat-manager-dead-channel-name = MUERTOS
chat-manager-admin-channel-name = ADMINISTRACIÓN

chat-manager-rate-limited = ¡Estás enviando mensajes demasiado rápido!
chat-manager-rate-limit-admin-announcement = Aviso: { $player } está enviando mensajes demasiado rápido.

## Verbos de habla del chat

chat-speech-verb-suffix-exclamation = !
chat-speech-verb-suffix-exclamation-strong = !!
chat-speech-verb-suffix-question = ?
chat-speech-verb-suffix-stutter = -
chat-speech-verb-suffix-mumble = ..

chat-speech-verb-name-none = Ninguno
chat-speech-verb-name-default = Predeterminado
chat-speech-verb-default = dice
chat-speech-verb-name-exclamation = Exclamación
chat-speech-verb-exclamation = exclama
chat-speech-verb-name-exclamation-strong = Grito
chat-speech-verb-exclamation-strong = grita
chat-speech-verb-name-question = Pregunta
chat-speech-verb-question = pregunta
chat-speech-verb-name-stutter = Tartamudeo
chat-speech-verb-stutter = tartamudea
chat-speech-verb-name-mumble = Murmullo
chat-speech-verb-mumble = murmura

chat-speech-verb-name-arachnid = Arácnido
chat-speech-verb-insect-1 = estridula
chat-speech-verb-insect-2 = chirría
chat-speech-verb-insect-3 = chasquea

chat-speech-verb-name-moth = Polilla
chat-speech-verb-winged-1 = aletea
chat-speech-verb-winged-2 = bate las alas
chat-speech-verb-winged-3 = zumba

chat-speech-verb-name-slime = Limo
chat-speech-verb-slime-1 = chapotea
chat-speech-verb-slime-2 = borbotea
chat-speech-verb-slime-3 = rezuma

chat-speech-verb-name-plant = Diona
chat-speech-verb-plant-1 = susurra entre las hojas
chat-speech-verb-plant-2 = se mece
chat-speech-verb-plant-3 = cruje

chat-speech-verb-name-robotic = Robótico
chat-speech-verb-robotic-1 = declara
chat-speech-verb-robotic-2 = pita
chat-speech-verb-robotic-3 = emite pitidos

chat-speech-verb-name-reptilian = Reptiliano
chat-speech-verb-reptilian-1 = sisea
chat-speech-verb-reptilian-2 = resopla
chat-speech-verb-reptilian-3 = resuella

chat-speech-verb-name-skeleton = Esqueleto
chat-speech-verb-skeleton-1 = traquetea
chat-speech-verb-skeleton-2 = castañetea
chat-speech-verb-skeleton-3 = rechina

chat-speech-verb-name-vox = Vox
chat-speech-verb-vox-1 = chilla
chat-speech-verb-vox-2 = vocifera
chat-speech-verb-vox-3 = croa

chat-speech-verb-name-canine = Canino
chat-speech-verb-canine-1 = ladra
chat-speech-verb-canine-2 = ladra con fuerza
chat-speech-verb-canine-3 = aúlla

chat-speech-verb-name-goat = Cabra
chat-speech-verb-goat-1 = bala
chat-speech-verb-goat-2 = gruñe
chat-speech-verb-goat-3 = berrea

chat-speech-verb-name-small-mob = Ratón
chat-speech-verb-small-mob-1 = chilla
chat-speech-verb-small-mob-2 = pía

chat-speech-verb-name-large-mob = Carpa
chat-speech-verb-large-mob-1 = ruge
chat-speech-verb-large-mob-2 = gruñe

chat-speech-verb-name-monkey = Mono
chat-speech-verb-monkey-1 = parlotea
chat-speech-verb-monkey-2 = chilla

chat-speech-verb-name-cluwne = Cluwne

chat-speech-verb-name-parrot = Loro
chat-speech-verb-parrot-1 = grazna
chat-speech-verb-parrot-2 = trina
chat-speech-verb-parrot-3 = gorjea

chat-speech-verb-cluwne-1 = ríe entre dientes
chat-speech-verb-cluwne-2 = suelta una carcajada
chat-speech-verb-cluwne-3 = ríe

chat-speech-verb-name-ghost = Fantasma
chat-speech-verb-ghost-1 = se queja
chat-speech-verb-ghost-2 = exhala
chat-speech-verb-ghost-3 = tararea
chat-speech-verb-ghost-4 = murmura

chat-speech-verb-name-electricity = Electricidad
chat-speech-verb-electricity-1 = crepita
chat-speech-verb-electricity-2 = zumba
chat-speech-verb-electricity-3 = chirría

chat-speech-verb-vulpkanin = Vulpkanin

chat-speech-verb-name-wawa = Wawa
chat-speech-verb-wawa-1 = entona
chat-speech-verb-wawa-2 = afirma
chat-speech-verb-wawa-3 = declara
chat-speech-verb-wawa-4 = reflexiona
