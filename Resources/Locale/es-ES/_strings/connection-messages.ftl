whitelist-not-whitelisted = No estás en la lista de acceso.
# Gestión de límites mínimo y máximo de jugadores
whitelist-playercount-invalid =
    { $min ->
        [0] La lista de acceso de este servidor solo se aplica cuando hay menos de { $max } jugadores; quizá puedas entrar más tarde.
       *[other]
            La lista de acceso de este servidor solo se aplica cuando hay más de { $min } { $max ->
                [2147483647] jugadores; quizá puedas entrar más tarde.
               *[other] y menos de { $max } jugadores; quizá puedas entrar más tarde.
            }
    }
whitelist-not-whitelisted-rp = No estás en la lista de acceso. Solicita acceso en el Discord oficial del servidor.

cmd-whitelistadd-desc = Añade a la lista de acceso del servidor al jugador cuyo nombre se indique.
cmd-whitelistadd-help = Uso: whitelistadd <nombre de usuario o ID de usuario>
cmd-whitelistadd-existing = ¡{ $username } ya está en la lista de acceso!
cmd-whitelistadd-added = Se ha añadido a { $username } a la lista de acceso.
cmd-whitelistadd-not-found = No se ha podido encontrar a «{ $username }».
cmd-whitelistadd-arg-player = [jugador]

cmd-whitelistremove-desc = Elimina de la lista de acceso del servidor al jugador cuyo nombre se indique.
cmd-whitelistremove-help = Uso: whitelistremove <nombre de usuario o ID de usuario>
cmd-whitelistremove-existing = ¡{ $username } no está en la lista de acceso!
cmd-whitelistremove-removed = Se ha eliminado a { $username } de la lista de acceso.
cmd-whitelistremove-not-found = No se ha podido encontrar a «{ $username }».
cmd-whitelistremove-arg-player = [jugador]

cmd-kicknonwhitelisted-desc = Expulsa del servidor a todos los jugadores que no estén en la lista de acceso.
cmd-kicknonwhitelisted-help = Uso: kicknonwhitelisted

ban-banned-permanent = Esta sanción solo podrá retirarse mediante una apelación.
ban-banned-permanent-appeal = Esta sanción solo podrá retirarse mediante una apelación. Puedes presentarla en { $link }.
ban-expires = La sanción durará { $duration } minutos y vencerá a las { $time } UTC.
ban-banned-1 = Tú, u otra persona que utiliza este equipo o conexión, tenéis prohibido jugar aquí.
ban-banned-2 = ID de la sanción: { $id }
ban-banned-3 = Motivo de la sanción: «{ $reason }»
ban-banned-4 = Quedarán registrados los intentos de eludir esta sanción, como crear una cuenta nueva.

soft-player-cap-full = ¡El servidor está lleno!
panic-bunker-account-denied = Este servidor se encuentra en modo búnker de emergencia, que suele activarse como precaución frente a incursiones. Temporalmente no se aceptan conexiones nuevas de cuentas que no cumplan ciertos requisitos. Inténtalo de nuevo más tarde.
panic-bunker-account-denied-reason = Este servidor se encuentra en modo búnker de emergencia, que suele activarse como precaución frente a incursiones. Temporalmente no se aceptan conexiones nuevas de cuentas que no cumplan ciertos requisitos. Inténtalo de nuevo más tarde. Motivo: «{ $reason }»
panic-bunker-account-reason-account = Tu cuenta de Space Station 14 es demasiado reciente. Debe tener más de { $minutes } minutos.
panic-bunker-account-reason-overall =
    Debes haber jugado en el servidor durante más de { $minutes } { $minutes ->
        [one] minuto
       *[other] minutos
    } en total.

whitelist-playtime = No has jugado el tiempo suficiente para entrar en este servidor. Necesitas al menos { $minutes } minutos de juego.
whitelist-player-count = En este momento el servidor no acepta jugadores. Inténtalo de nuevo más tarde.
whitelist-notes = Tienes demasiadas notas de administración para entrar en este servidor. Puedes consultarlas escribiendo /adminremarks en el chat.
whitelist-manual = No estás en la lista de acceso de este servidor.
whitelist-blacklisted = Estás en la lista de exclusión de este servidor.
whitelist-always-deny = No tienes permiso para entrar en este servidor.
whitelist-fail-prefix = Sin acceso: { $msg }

cmd-blacklistadd-desc = Añade a la lista de exclusión del servidor al jugador cuyo nombre se indique.
cmd-blacklistadd-help = Uso: blacklistadd <nombre de usuario>
cmd-blacklistadd-existing = ¡{ $username } ya está en la lista de exclusión!
cmd-blacklistadd-added = Se ha añadido a { $username } a la lista de exclusión.
cmd-blacklistadd-not-found = No se ha podido encontrar a «{ $username }».
cmd-blacklistadd-arg-player = [jugador]

cmd-blacklistremove-desc = Elimina de la lista de exclusión del servidor al jugador cuyo nombre se indique.
cmd-blacklistremove-help = Uso: blacklistremove <nombre de usuario>
cmd-blacklistremove-existing = ¡{ $username } no está en la lista de exclusión!
cmd-blacklistremove-removed = Se ha eliminado a { $username } de la lista de exclusión.
cmd-blacklistremove-not-found = No se ha podido encontrar a «{ $username }».
cmd-blacklistremove-arg-player = [jugador]

baby-jail-account-denied = Este servidor está pensado para jugadores nuevos y para quienes quieran ayudarlos. No acepta conexiones nuevas de cuentas demasiado antiguas o que no estén en la lista de acceso. Prueba otros servidores y descubre todo lo que ofrece Space Station 14. ¡Diviértete!
baby-jail-account-denied-reason = Este servidor está pensado para jugadores nuevos y para quienes quieran ayudarlos. No acepta conexiones nuevas de cuentas demasiado antiguas o que no estén en la lista de acceso. Prueba otros servidores y descubre todo lo que ofrece Space Station 14. ¡Diviértete! Motivo: «{ $reason }»
baby-jail-account-reason-account = Tu cuenta de Space Station 14 es demasiado antigua. Debe tener menos de { $minutes } minutos.
baby-jail-account-reason-overall = Tu tiempo de juego total en el servidor debe ser inferior a { $minutes } minutos.

generic-misconfigured = El servidor está mal configurado y no acepta jugadores. Ponte en contacto con su responsable e inténtalo de nuevo más tarde.

ipintel-server-ratelimited = Este servidor utiliza un sistema de auditoría con verificación externa, pero ha alcanzado el límite máximo de comprobaciones del servicio. Informa al equipo de administración para obtener ayuda o inténtalo de nuevo más tarde.
ipintel-unknown = Este servidor utiliza un sistema de auditoría con verificación externa, pero se ha producido un error al comprobar tu conexión. Informa al equipo de administración para obtener ayuda o inténtalo de nuevo más tarde.
ipintel-suspicious = Parece que intentas conectarte mediante un centro de datos, un proxy, una VPN u otra conexión sospechosa. Por motivos administrativos no permitimos jugar desde estas conexiones. Si tienes activada una VPN o un servicio similar, desactívalo e intenta conectarte de nuevo. Si crees que se trata de un error o necesitas utilizar estos servicios, ponte en contacto con el equipo de administración.

hwid-required = Tu cliente se ha negado a enviar un identificador de hardware. Ponte en contacto con el equipo de administración para obtener ayuda.
