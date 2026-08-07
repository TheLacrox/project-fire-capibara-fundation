entity-effect-guidebook-knockdown =
    { $type ->
        [update] { $chance ->
                    [1] Provoca
                    *[other] Puede provocar
                 } {LOC($key)} durante al menos {NATURALFIXED($time, 3)} { $time ->
                    [one] segundo
                    *[other] segundos
                 }, sin acumulación
        [add]    { $chance ->
                    [1] Provoca
                    *[other] Puede provocar
                 } un derribo durante al menos {NATURALFIXED($time, 3)} { $time ->
                    [one] segundo
                    *[other] segundos
                 }, con acumulación
        *[set]   { $chance ->
                    [1] Provoca
                    *[other] Puede provocar
                 } un derribo durante al menos {NATURALFIXED($time, 3)} { $time ->
                    [one] segundo
                    *[other] segundos
                 }, sin acumulación
        [remove] { $chance ->
                    [1] Elimina
                    *[other] Puede eliminar
                 } {NATURALFIXED($time, 3)} { $time ->
                    [one] segundo
                    *[other] segundos
                 } de derribo
    }

entity-effect-guidebook-area-reaction =
    { $chance ->
        [1] Provoca
        *[other] Puede provocar
    } una reacción de humo o espuma durante {NATURALFIXED($duration, 3)} { $duration ->
        [one] segundo
        *[other] segundos
    }
