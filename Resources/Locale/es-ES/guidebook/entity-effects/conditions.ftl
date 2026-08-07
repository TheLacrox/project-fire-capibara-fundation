entity-condition-guidebook-total-damage =
    { $max ->
        [2147483648] tiene al menos {NATURALFIXED($min, 2)} de daño total
        *[other] { $min ->
                    [0] tiene como máximo {NATURALFIXED($max, 2)} de daño total
                    *[other] tiene entre {NATURALFIXED($min, 2)} y {NATURALFIXED($max, 2)} de daño total
                 }
    }

entity-condition-guidebook-type-damage =
    { $max ->
        [2147483648] tiene al menos {NATURALFIXED($min, 2)} de daño {$type}
        *[other] { $min ->
                    [0] tiene como máximo {NATURALFIXED($max, 2)} de daño {$type}
                    *[other] tiene entre {NATURALFIXED($min, 2)} y {NATURALFIXED($max, 2)} de daño {$type}
                 }
    }

entity-condition-guidebook-group-damage =
    { $max ->
        [2147483648] tiene al menos {NATURALFIXED($min, 2)} de daño del grupo {$type}.
        *[other] { $min ->
                    [0] tiene como máximo {NATURALFIXED($max, 2)} de daño del grupo {$type}.
                    *[other] tiene entre {NATURALFIXED($min, 2)} y {NATURALFIXED($max, 2)} de daño del grupo {$type}
                 }
    }

entity-condition-guidebook-total-hunger =
    { $max ->
        [2147483648] el objetivo tiene al menos {NATURALFIXED($min, 2)} de hambre total
        *[other] { $min ->
                    [0] el objetivo tiene como máximo {NATURALFIXED($max, 2)} de hambre total
                    *[other] el objetivo tiene entre {NATURALFIXED($min, 2)} y {NATURALFIXED($max, 2)} de hambre total
                 }
    }

entity-condition-guidebook-reagent-threshold =
    { $max ->
        [2147483648] hay al menos {NATURALFIXED($min, 2)}u de {$reagent}
        *[other] { $min ->
                    [0] hay como máximo {NATURALFIXED($max, 2)}u de {$reagent}
                    *[other] hay entre {NATURALFIXED($min, 2)}u y {NATURALFIXED($max, 2)}u de {$reagent}
                 }
    }

entity-condition-guidebook-mob-state-condition =
    la criatura está { $state }

entity-condition-guidebook-job-condition =
    el puesto del objetivo es { $job }

entity-condition-guidebook-solution-temperature =
    la temperatura de la solución { $max ->
            [2147483648] es de al menos {NATURALFIXED($min, 2)}k
            *[other] { $min ->
                        [0] es de {NATURALFIXED($max, 2)}k como máximo
                        *[other] está entre {NATURALFIXED($min, 2)}k y {NATURALFIXED($max, 2)}k
                     }
    }

entity-condition-guidebook-body-temperature =
    la temperatura del cuerpo { $max ->
            [2147483648] es de al menos {NATURALFIXED($min, 2)}k
            *[other] { $min ->
                        [0] es de {NATURALFIXED($max, 2)}k como máximo
                        *[other] está entre {NATURALFIXED($min, 2)}k y {NATURALFIXED($max, 2)}k
                     }
    }

entity-condition-guidebook-organ-type =
    el órgano metabolizador { $shouldhave ->
                                [true] es
                                *[false] no es
                           } un órgano de tipo {$name}

entity-condition-guidebook-has-tag =
    el objetivo { $invert ->
                 [true] no tiene
                 *[false] tiene
                } la etiqueta {$tag}

entity-condition-guidebook-this-reagent = este reactivo

entity-condition-guidebook-breathing =
    el metabolizador { $isBreathing ->
                [true] respira con normalidad
                *[false] se está asfixiando
               }

entity-condition-guidebook-internals =
    el metabolizador { $usingInternals ->
                [true] está usando los internos
                *[false] respira aire atmosférico
               }
