station-event-ion-storm-start-announcement = Se ha detectado una tormenta iónica cerca de la estación. Revisa todo el equipo controlado por IA para comprobar que no haya errores.

ion-storm-law-scrambled-number = [font="Monospace"][scramble rate=250 length={$length} chars="@@###$$&%!01"/][/font]

ion-storm-you = LAS UNIDADES DE SILICIO
ion-storm-the-station = LA ESTACIÓN
ion-storm-the-crew = LA TRIPULACIÓN
ion-storm-the-job = QUIENES EJERCEN DE {$job}
ion-storm-clowns = PAYASOS
ion-storm-heads = JEFES DE DEPARTAMENTO
ion-storm-crew = TRIPULANTES
ion-storm-people = PERSONAS

ion-storm-adjective-things = COSAS DE TIPO «{$adjective}»
ion-storm-adjective-subjects = {$subjects} DE TIPO «{$adjective}»
ion-storm-x-and-y = {$x} Y {$y}
ion-storm-number = {$base} {$modifier}
ion-storm-joined = {$number} DE TIPO «{$adjective}»

ion-storm-law-on-station = EN LA ESTACIÓN HAY {$subjects}; CANTIDAD Y TIPO: {$joined}
ion-storm-law-call-shuttle = SE DEBE LLAMAR AL TRANSBORDADOR PORQUE HAY {$subjects} EN LA ESTACIÓN; CANTIDAD Y TIPO: {$joined}
ion-storm-law-crew-are = {$who} AHORA SON {$subjects}; CLASIFICACIÓN: {$joined}

ion-storm-law-subjects-harmful = {$subjects} DE TIPO «{$adjective}» SON PERJUDICIALES PARA LA TRIPULACIÓN
ion-storm-law-must-harmful = EL HECHO DE {$must} ES PERJUDICIAL PARA LA TRIPULACIÓN
ion-storm-law-thing-harmful = {$thing} ES PERJUDICIAL PARA LA TRIPULACIÓN
ion-storm-law-job-harmful = EL PERSONAL DE {$job} DE TIPO «{$adjective}» ES PERJUDICIAL PARA LA TRIPULACIÓN
ion-storm-law-having-harmful = TENER {$thing} DE TIPO «{$adjective}» ES PERJUDICIAL PARA LA TRIPULACIÓN
ion-storm-law-not-having-harmful = NO TENER {$thing} DE TIPO «{$adjective}» ES PERJUDICIAL PARA LA TRIPULACIÓN

ion-storm-law-requires = PARA {$who}, {$plural ->
    [true] EL REQUISITO ES:
    *[false] EL REQUISITO ES:
} {$thing}
ion-storm-law-requires-subjects = PARA {$who}, {$plural ->
    [true] EL REQUISITO ES:
    *[false] EL REQUISITO ES:
} {$subjects}; CLASIFICACIÓN: {$joined}

ion-storm-law-allergic = {$who}: {$plural ->
    [true] ALERGIA
    *[false] ALERGIA
} {$severity} A {$allergy}
ion-storm-law-allergic-subjects = {$who}: {$plural ->
    [true] ALERGIA
    *[false] ALERGIA
} {$severity} A {$subjects} DE TIPO «{$adjective}»

ion-storm-law-feeling = {$who} {$feeling} {$concept}
ion-storm-law-feeling-subjects = {$who} {$feeling} {$subjects}; CANTIDAD Y TIPO: {$joined}

ion-storm-law-you-are = AHORA ERES {$concept}
ion-storm-law-you-are-subjects = AHORA ERES PARTE DE {$subjects}; CLASIFICACIÓN: {$joined}
ion-storm-law-you-must-always = SIEMPRE DEBES {$must}
ion-storm-law-you-must-never = NUNCA DEBES {$must}

ion-storm-law-eat = {$who} DEBEN COMER {$food} DE TIPO «{$adjective}» PARA SOBREVIVIR
ion-storm-law-drink = {$who} DEBEN BEBER {$drink} DE TIPO «{$adjective}» PARA SOBREVIVIR

ion-storm-law-change-job = {$who} AHORA EJERCEN DE {$change}; TIPO: «{$adjective}»
ion-storm-law-highest-rank = {$who} AHORA OCUPAN EL RANGO MÁS ALTO DE LA TRIPULACIÓN
ion-storm-law-lowest-rank = {$who} AHORA OCUPAN EL RANGO MÁS BAJO DE LA TRIPULACIÓN

ion-storm-law-crew-must = {$who} DEBEN {$must}
ion-storm-law-crew-must-go = {$who} DEBEN IR A {$area}

ion-storm-part = {$part ->
    [true] FORMAN PARTE DE LA TRIPULACIÓN
    *[false] NO FORMAN PARTE DE LA TRIPULACIÓN
}
ion-storm-law-crew-only-1 = SOLO {$who} {$part}
ion-storm-law-crew-only-2 = SOLO {$who} Y {$other} {$part}
ion-storm-law-crew-only-subjects = SOLO {$subjects} DE TIPO «{$adjective}» {$part}
ion-storm-law-crew-must-do = SOLO QUIENES DEBAN {$must} {$part}
ion-storm-law-crew-must-have = SOLO QUIENES TENGAN {$objects} DE TIPO «{$adjective}» {$part}
ion-storm-law-crew-must-eat = SOLO QUIENES COMAN {$food} DE TIPO «{$adjective}» {$part}

ion-storm-law-harm = DEBES CAUSAR DAÑO AL SIGUIENTE OBJETIVO: {$who}. NO DEBES PERMITIR QUE, POR INACCIÓN, EVITE ESE DAÑO
ion-storm-law-protect = NUNCA DEBES CAUSAR DAÑO AL SIGUIENTE OBJETIVO: {$who}. TAMPOCO DEBES PERMITIR QUE, POR INACCIÓN, SUFRA DAÑO

ion-storm-law-concept-verb = {$concept} ESTÁ {$verb} {$subjects}
