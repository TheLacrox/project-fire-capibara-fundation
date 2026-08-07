lathe-menu-title = Menú del torno
lathe-menu-queue = Cola
lathe-menu-server-list = Lista de servidores
lathe-menu-sync = Sincronizar
lathe-menu-search-designs = Buscar diseños
lathe-menu-category-all = Todo
lathe-menu-search-filter = Filtro:
lathe-menu-amount = Cantidad:
lathe-menu-recipe-count = { $count ->
    [one] { $count } receta
   *[other] { $count } recetas
}
lathe-menu-reagent-slot-examine = Tiene una ranura lateral para un vaso de precipitados.
lathe-reagent-dispense-no-container = ¡El líquido de { $name } se derrama por el suelo!
lathe-menu-result-reagent-display = { $reagent } ({ $amount } u)
lathe-menu-material-display = { $material } ({ $amount })
lathe-menu-tooltip-display = { $amount } de { $material }
lathe-menu-description-display = [italic]{ $description }[/italic]
lathe-menu-material-amount = { $amount ->
    [one] { NATURALFIXED($amount, 2) } { $unit }
   *[other] { NATURALFIXED($amount, 2) } { MAKEPLURAL($unit) }
}
lathe-menu-material-amount-missing =
    { $amount ->
        [one] { NATURALFIXED($amount, 2) } { $unit } de { $material }
       *[other] { NATURALFIXED($amount, 2) } { MAKEPLURAL($unit) } de { $material }
    } ([color=red]{ $missingAmount ->
        [one] falta { NATURALFIXED($missingAmount, 2) } { $unit }
       *[other] faltan { NATURALFIXED($missingAmount, 2) } { MAKEPLURAL($unit) }
    }[/color])
lathe-menu-no-materials-message = No hay materiales cargados.
lathe-menu-silo-linked-message = Silo vinculado
lathe-menu-fabricating-message = Fabricando…
lathe-menu-materials-title = Materiales
lathe-menu-queue-title = Cola de fabricación
lathe-menu-delete-fabricating-tooltip = Cancela la impresión del objeto actual.
lathe-menu-delete-item-tooltip = Cancela la impresión de este lote.
lathe-menu-move-up-tooltip = Adelanta este lote en la cola.
lathe-menu-move-down-tooltip = Retrasa este lote en la cola.
lathe-menu-item-single = { $index }. { $name }
lathe-menu-item-batch = { $index }. { $name } ({ $printed }/{ $total })
