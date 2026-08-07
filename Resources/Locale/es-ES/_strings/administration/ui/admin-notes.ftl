# Interfaz
admin-notes-title = Notas sobre { $player }
admin-notes-new-note = Nueva nota
admin-notes-show-more = Mostrar más
admin-notes-for = Nota sobre: { $player }
admin-notes-id = ID: { $id }
admin-notes-type = Tipo: { $type }
admin-notes-severity = Gravedad: { $severity }
admin-notes-secret = Secreta
admin-notes-notsecret = No secreta
admin-notes-expires = Caduca el: { $expires }
admin-notes-expires-never = No caduca
admin-notes-edited-never = Nunca
admin-notes-round-id = ID de la ronda: { $id }
admin-notes-round-id-unknown = ID de la ronda: desconocido
admin-notes-created-by = Creada por: { $author }
admin-notes-created-at = Creada el: { $date }
admin-notes-last-edited-by = Última edición de: { $author }
admin-notes-last-edited-at = Última edición el: { $date }
admin-notes-edit = Editar
admin-notes-delete = Eliminar
admin-notes-hide = Ocultar
admin-notes-delete-confirm = Confirmar eliminación
admin-notes-edited = Última edición de { $author } el { $date }
admin-notes-unbanned = Sanción retirada por { $admin } el { $date }
admin-notes-message-desc = [color=white]Desde tu última partida en este servidor has recibido { $count ->
    [1] un mensaje administrativo
   *[other] { $count } mensajes administrativos
}.[/color]
admin-notes-message-admin = De [bold]{ $admin }[/bold], escrito el { TOSTRING($date, "f") }:
admin-notes-message-wait =
    El botón para aceptar se habilitará dentro de { $time ->
        [1] { $time } segundo.
       *[other] { $time } segundos.
    }
admin-notes-message-accept = Descartar permanentemente
admin-notes-message-dismiss = Descartar por ahora
admin-notes-message-seen = Visto
admin-notes-banned-from = Sancionado en
admin-notes-the-server = el servidor
admin-notes-permanently = permanentemente
admin-notes-unknown-server = Desconocido
admin-notes-unknown-round = Ronda desconocida
admin-notes-round = Ronda { $round }
admin-notes-unknown-role = puesto desconocido
admin-notes-for-duration = durante { $duration }
admin-notes-days =
    { $days ->
        [1] { $days } día
       *[other] { $days } días
    }
admin-notes-hours =
    { $hours ->
        [1] { $hours } hora
       *[other] { $hours } horas
    }
admin-notes-minutes =
    { $minutes ->
        [1] { $minutes } minuto
       *[other] { $minutes } minutos
    }

# Editor de notas
admin-note-editor-title-new = Creando una nota sobre { $player }
admin-note-editor-title-existing = Editando la nota { $id } sobre { $player }, creada por { $author }
admin-note-editor-pop-out = Abrir en otra ventana
admin-note-editor-secret = ¿Secreta?
admin-note-editor-secret-tooltip = Actívala para impedir que el jugador vea la nota
admin-note-editor-type-note = Nota
admin-note-editor-type-message = Mensaje
admin-note-editor-type-watchlist = Seguimiento
admin-note-editor-type-server-ban = Sanción del servidor
admin-note-editor-type-role-ban = Veto de puesto
admin-note-editor-severity-select = Seleccionar
admin-note-editor-severity-none = Ninguna
admin-note-editor-severity-low = Baja
admin-note-editor-severity-medium = Media
admin-note-editor-severity-high = Alta
admin-note-editor-expiry-checkbox = ¿Permanente?
admin-note-editor-expiry-checkbox-tooltip = Actívala para que la nota no caduque
admin-note-editor-expiry-label = Caduca dentro de:
admin-note-editor-expiry-label-params = Caduca el { $date } (dentro de { $expiresIn })
admin-note-editor-expiry-label-expired = Caducada
admin-note-editor-expiry-placeholder = Introduce el tiempo hasta la caducidad como número entero.
admin-note-editor-submit = Guardar
admin-note-editor-submit-confirm = ¿Confirmas la operación?

# Unidades de tiempo
admin-note-button-minutes = Minutos
admin-note-button-hours = Horas
admin-note-button-days = Días
admin-note-button-weeks = Semanas
admin-note-button-months = Meses
admin-note-button-years = Años
admin-note-button-centuries = Siglos

# Verbo
admin-notes-verb-text = Abrir las notas administrativas

# Seguimiento y mensaje al iniciar sesión
admin-notes-watchlist = Seguimiento de { $player }: { $message }
admin-notes-new-message = Has recibido un mensaje administrativo de { $admin }: { $message }
admin-notes-fallback-admin-name = [Sistema]

# Observaciones administrativas
admin-remarks-command-description = Abre la página de observaciones administrativas
admin-remarks-command-error = Las observaciones administrativas están deshabilitadas
admin-remarks-title = Observaciones administrativas

# Otros
system-user = [Sistema]
