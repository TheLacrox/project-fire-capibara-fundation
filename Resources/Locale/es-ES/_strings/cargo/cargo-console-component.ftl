## Interfaz
cargo-console-menu-title = Consola de pedidos de Logística
cargo-console-menu-account-name-label = Cuenta:{ " " }
cargo-console-menu-account-name-none-text = Ninguna
cargo-console-menu-account-name-format = [bold][color={ $color }]{ $name }[/color][/bold] [font="Monospace"]\[{ $code }\][/font]
cargo-console-menu-shuttle-name-label = Nombre del transbordador:{ " " }
cargo-console-menu-shuttle-name-none-text = Ninguno
cargo-console-menu-points-label = Saldo:{ " " }
cargo-console-menu-points-amount = ${ $amount }
cargo-console-menu-shuttle-status-label = Estado del transbordador:{ " " }
cargo-console-menu-shuttle-status-away-text = Fuera
cargo-console-menu-order-capacity-label = Capacidad de pedidos:{ " " }
cargo-console-menu-call-shuttle-button = Activar teleplataforma
cargo-console-menu-permissions-button = Permisos
cargo-console-menu-categories-label = Categorías:{ " " }
cargo-console-menu-search-bar-placeholder = Buscar
cargo-console-menu-requests-label = Solicitudes
cargo-console-menu-orders-label = Pedidos
cargo-console-menu-order-reason-description = Motivo: { $reason }
cargo-console-menu-populate-categories-all-text = Todas
cargo-console-menu-populate-orders-cargo-order-row-product-name-text = { $productName } (x{ $orderAmount }), solicitado por { $orderRequester } con cargo a [color={ $accountColor }]{ $account }[/color]
cargo-console-menu-cargo-order-row-approve-button = Aprobar
cargo-console-menu-cargo-order-row-cancel-button = Cancelar
cargo-console-menu-tab-title-orders = Pedidos
cargo-console-menu-tab-title-funds = Transferencias
cargo-console-menu-account-action-transfer-limit = [bold]Límite de transferencia:[/bold] ${ $limit }
cargo-console-menu-account-action-transfer-limit-unlimited-notifier = [color=gold](Sin límite)[/color]
cargo-console-menu-account-action-select = [bold]Operación de la cuenta:[/bold]
cargo-console-menu-account-action-amount = [bold]Cantidad:[/bold] $
cargo-console-menu-account-action-button = Transferir
cargo-console-menu-toggle-account-lock-button = Alternar límite de transferencia
cargo-console-menu-account-action-option-withdraw = Retirar efectivo
cargo-console-menu-account-action-option-transfer = Transferir fondos a { $code }

# Pedidos
cargo-console-order-not-allowed = Acceso denegado
cargo-console-station-not-found = No hay ningún complejo disponible
cargo-console-invalid-product = ID de producto no válido
cargo-console-too-many = Hay demasiados pedidos aprobados
cargo-console-snip-snip = El pedido se ha reducido a la capacidad disponible
cargo-console-insufficient-funds = Fondos insuficientes (se necesitan { $cost })
cargo-console-unfulfilled = No hay espacio para completar el pedido
cargo-console-trade-station = Enviado a { $destination }
cargo-console-unlock-approved-order-broadcast = [bold]{ $productName } x{ $orderAmount }[/bold], con un coste de [bold]{ $cost }[/bold], ha sido aprobado por [bold]{ $approver }[/bold]
cargo-console-fund-withdraw-broadcast = [bold]{ $name } ha retirado { $amount } spesos de { $name1 } \[{ $code1 }\]
cargo-console-fund-transfer-broadcast = [bold]{ $name } ha transferido { $amount } spesos de { $name1 } \[{ $code1 }\] a { $name2 } \[{ $code2 }\][/bold]
cargo-console-fund-transfer-user-unknown = Desconocido

cargo-console-paper-reason-default = Ninguno
cargo-console-paper-approver-default = Aprobación propia
cargo-console-paper-print-name = Pedido n.º { $orderNumber }
cargo-console-paper-print-text = [head=2]Pedido n.º { $orderNumber }[/head]
    { "[bold]Artículo:[/bold]" } { $itemName } (x{ $orderQuantity })
    { "[bold]Solicitado por:[/bold]" } { $requester }

    { "[head=3]Información del pedido[/head]" }
    { "[bold]Pagador:[/bold]" } { $account } [font="Monospace"]\[{ $accountcode }\][/font]
    { "[bold]Aprobado por:[/bold]" } { $approver }
    { "[bold]Motivo:[/bold]" } { $reason }

# Consola del transbordador de carga
cargo-shuttle-console-menu-title = Consola del transbordador de carga
cargo-shuttle-console-station-unknown = Desconocido
cargo-shuttle-console-shuttle-not-found = No encontrado
cargo-shuttle-console-organics = Se han detectado formas de vida orgánicas en el transbordador
cargo-no-shuttle = No se ha encontrado ningún transbordador de carga.

# Consola de reparto de fondos
cargo-funding-alloc-console-menu-title = Consola de reparto de fondos
cargo-funding-alloc-console-label-account = [bold]Cuenta[/bold]
cargo-funding-alloc-console-label-code = [bold] Código [/bold]
cargo-funding-alloc-console-label-balance = [bold] Saldo [/bold]
cargo-funding-alloc-console-label-cut = [bold] Distribución de ingresos (%) [/bold]

cargo-funding-alloc-console-label-primary-cut = Porcentaje de Logística de los fondos procedentes de fuentes distintas de las cajas de seguridad (%):
cargo-funding-alloc-console-label-lockbox-cut = Porcentaje de Logística de las ventas de cajas de seguridad (%):

cargo-funding-alloc-console-label-help-non-adjustible = Logística recibe el { $percent } % de los beneficios de las ventas ajenas a las cajas de seguridad. El resto se reparte como se indica a continuación:
cargo-funding-alloc-console-label-help-adjustible = Los fondos restantes de fuentes distintas de las cajas de seguridad se reparten como se indica a continuación:
cargo-funding-alloc-console-button-save = Guardar cambios
cargo-funding-alloc-console-label-save-fail = [bold]La distribución de ingresos no es válida.[/bold] [color=red]({ $pos ->
    [1] +
   *[-1] -
}{ $val } %)[/color]

# Plantilla del comprobante
cargo-acquisition-slip-body = [head=3]Detalles del activo[/head]
    { "[bold]Producto:[/bold]" } { $product }
    { "[bold]Descripción:[/bold]" } { $description }
    { "[bold]Coste unitario:[/bold]" } ${ $unit }
    { "[bold]Cantidad:[/bold]" } { $amount }
    { "[bold]Coste:[/bold]" } ${ $cost }

    { "[head=3]Detalles de la compra[/head]" }
    { "[bold]Solicitante:[/bold]" } { $orderer }
    { "[bold]Motivo:[/bold]" } { $reason }
