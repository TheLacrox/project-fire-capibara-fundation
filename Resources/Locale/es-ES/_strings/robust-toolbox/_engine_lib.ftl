# Usado internamente por THE().
zzzz-the = { PROPER($ent) ->
    [true] { $ent }
   *[false] { GENDER($ent) ->
        [female] la { $ent }
       *[other] el { $ent }
    }
}

# Forma contraída de «a» + artículo para entidades españolas.
zzzz-at-the = { PROPER($ent) ->
    [true] a { $ent }
   *[false] { GENDER($ent) ->
        [female] a la { $ent }
       *[other] al { $ent }
    }
}

# Usado internamente por SUBJECT().
zzzz-subject-pronoun = { GENDER($ent) ->
    [male] él
    [female] ella
    [epicene] esa persona
   *[neuter] eso
}

# Usado internamente por OBJECT().
zzzz-object-pronoun = { GENDER($ent) ->
    [male] lo
    [female] la
    [epicene] la
   *[neuter] lo
}

# Usado internamente por DAT-OBJ().
zzzz-dat-object = le

# Usado internamente por GENITIVE().
zzzz-genitive = { GENDER($ent) ->
    [male] de él
    [female] de ella
    [epicene] de esa persona
   *[neuter] de eso
}

# Usado internamente por POSS-PRONOUN().
zzzz-possessive-pronoun = { GENDER($ent) ->
    [male] de él
    [female] de ella
    [epicene] de esa persona
   *[neuter] de eso
}

# Usado internamente por POSS-ADJ().
zzzz-possessive-adjective = su

# Usado internamente por REFLEXIVE().
zzzz-reflexive-pronoun = { GENDER($ent) ->
    [male] sí mismo
    [female] sí misma
   *[other] por sí
}

# Usado internamente por CONJUGATE-BE().
zzzz-conjugate-be = está

# Usado internamente por CONJUGATE-HAVE().
zzzz-conjugate-have = tiene

# Usado internamente por CONJUGATE-BASIC().
zzzz-conjugate-basic = { $second }
