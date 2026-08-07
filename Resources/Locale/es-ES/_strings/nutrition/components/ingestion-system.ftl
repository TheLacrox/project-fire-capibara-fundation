### Mensajes de interacción

# Sistema

## Al intentar ingerir algo sin el utensilio necesario... pero hay que tenerlo en la mano
ingestion-you-need-to-hold-utensil = ¡Necesitas tener en la mano un utensilio de tipo {$utensil} para comer eso!

ingestion-try-use-is-empty = ¡{CAPITALIZE(THE($entity))} está vacío!
ingestion-try-use-wrong-utensil = No puedes {$verb} {THE($food)} con {THE($utensil)}.

ingestion-remove-mask = Primero tienes que quitarte {THE($entity)}.

## Ingestión fallida

ingestion-you-cannot-ingest-any-more = ¡No puedes {$verb} más!
ingestion-other-cannot-ingest-any-more = ¡{CAPITALIZE(SUBJECT($target))} no puede {$verb} más!

ingestion-cant-digest = ¡No puedes digerir {THE($entity)}!
ingestion-cant-digest-other = ¡{CAPITALIZE(SUBJECT($target))} no puede digerir {THE($entity)}!

## Verbos de acción, no confundir con los verbos de interacción

ingestion-verb-food = Comer
ingestion-verb-drink = Beber

# Componente Edible

edible-nom = Ñam. {$flavors}
edible-nom-other = Ñam.
edible-slurp = Sorbo. {$flavors}
edible-slurp-other = Sorbo.
edible-swallow = Te tragas { THE($food) }
edible-gulp = Glup. {$flavors}
edible-gulp-other = Glup.

edible-has-used-storage = No puedes {$verb} { THE($food) } si tiene algo guardado dentro.

## Sustantivos

edible-noun-edible = comestible
edible-noun-food = comida
edible-noun-drink = bebida
edible-noun-pill = pastilla

## Verbos

edible-verb-edible = ingerir
edible-verb-food = comer
edible-verb-drink = beber
edible-verb-pill = tragar

## Alimentación forzada

edible-force-feed = ¡{CAPITALIZE(THE($user))} está intentando hacerte {$verb} algo!
edible-force-feed-success = ¡{CAPITALIZE(THE($user))} te ha obligado a {$verb} algo! {$flavors}
edible-force-feed-success-user = Has conseguido dar de comer a {THE($target)}
