server-ban-string-infinity = Para siempre
server-ban-no-name = No encontrado. ({ $hwid })
server-time-ban =
    Sanción temporal de { $mins } { $mins ->
        [one] minuto
       *[other] minutos
    }.
server-perma-ban = Sanción permanente.
server-role-ban =
    Veto de puesto temporal de { $mins } { $mins ->
        [one] minuto
       *[other] minutos
    }.
server-perma-role-ban = Veto de puesto permanente.
server-time-ban-string =
    > **Persona sancionada**
    > **Usuario:** ``{ $targetName }``
    > **Discord:** { $targetLink }

    > **Administrador**
    > **Usuario:** ``{ $adminName }``
    > **Discord:** { $adminLink }

    > **Fecha de imposición:** { $TimeNow }
    > **Caduca el:** { $expiresString }

    > **Motivo:** { $reason }

    > **Gravedad:** { $severity }
server-ban-footer = { $server } | Ronda: #{ $round }
server-perma-ban-string =
    > **Persona sancionada**
    > **Usuario:** ``{ $targetName }``
    > **Discord:** { $targetLink }

    > **Administrador**
    > **Usuario:** ``{ $adminName }``
    > **Discord:** { $adminLink }

    > **Fecha de imposición:** { $TimeNow }

    > **Motivo:** { $reason }

    > **Gravedad:** { $severity }
server-role-ban-string =
    > **Persona sancionada**
    > **Usuario:** ``{ $targetName }``
    > **Discord:** { $targetLink }

    > **Administrador**
    > **Usuario:** ``{ $adminName }``
    > **Discord:** { $adminLink }

    > **Fecha de imposición:** { $TimeNow }
    > **Caduca el:** { $expiresString }

    > **Puestos:** { $roles }

    > **Motivo:** { $reason }

    > **Gravedad:** { $severity }
server-perma-role-ban-string =
    > **Persona sancionada**
    > **Usuario:** ``{ $targetName }``
    > **Discord:** ``{ $targetLink }``

    > **Administrador**
    > **Usuario:** ``{ $adminName }``
    > **Discord:** { $adminLink }

    > **Fecha de imposición:** { $TimeNow }

    > **Puestos:** { $roles }

    > **Motivo:** { $reason }

    > **Gravedad:** { $severity }
