examine-fear-state-anxiety = [color=lightblue]{ CAPITALIZE(gender-based-third-form) } parece sentir ansiedad[/color]
examine-fear-state-fear = [color=lightblue]{ CAPITALIZE(gender-based-third-form-case) } mirada refleja miedo[/color]
examine-fear-state-terror = [color=lightblue]{ CAPITALIZE(gender-based-third-form) } parece haber perdido la razón[/color]
examine-fear-state-none-dead = [color=lightblue]{ CAPITALIZE(gender-based-third-form) } parece estar en calma, como si la muerte hubiese llegado sin previo aviso[/color]
examine-fear-state-anxiety-dead = [color=lightblue]En { gender-based-third-form-case } mirada apagada permanece la última expresión de ansiedad, observando tu alma aún viva[/color]
examine-fear-state-fear-dead = [color=lightblue]En { gender-based-third-form-case } mirada, con los ojos abiertos de par en par, ha quedado grabado el instante consciente que fue el último de su vida[/color]
examine-fear-state-terror-dead = [color=lightblue]{ CAPITALIZE(gender-based-third-form-case) } boca ha quedado congelada en un grito mudo y sus ojos contemplan un vacío que nadie debería haber visto[/color]
gender-based-third-form =
    { GENDER($target) ->
        [male] él
        [female] ella
        [epicene] esa persona
       *[neuter] eso
    }
gender-based-third-form-case =
    { GENDER($target) ->
        [male] su
        [female] su
        [epicene] su
       *[neuter] su
    }
