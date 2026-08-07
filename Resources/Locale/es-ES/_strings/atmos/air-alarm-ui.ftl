# Interfaz

## Ventana

air-alarm-ui-title = Alarma atmosférica
air-alarm-ui-access-denied = Acceso insuficiente.
air-alarm-ui-window-pressure-label = Presión
air-alarm-ui-window-temperature-label = Temperatura
air-alarm-ui-window-alarm-state-label = Estado
air-alarm-ui-window-address-label = Dirección
air-alarm-ui-window-device-count-label = Dispositivos totales
air-alarm-ui-window-resync-devices-label = Resincronizar
air-alarm-ui-window-mode-label = Modo
air-alarm-ui-window-mode-select-locked-label = [bold][color=red] Error del selector de modo. [/color][/bold]
air-alarm-ui-window-auto-mode-label = Modo automático

-air-alarm-state-name = { $state ->
    [normal] Normal
    [warning] Advertencia
    [danger] Peligro
    [emagged] Saboteada
   *[invalid] No válido
}

air-alarm-ui-window-listing-title = { $address }: { $state ->
    [normal] Normal
    [warning] Advertencia
    [danger] Peligro
    [emagged] Saboteada
   *[invalid] No válido
}
air-alarm-ui-window-pressure = { $pressure } kPa
air-alarm-ui-window-pressure-indicator = Presión: [color={ $color }]{ $pressure } kPa[/color]
air-alarm-ui-window-temperature = { $tempC } °C ({ $temperature } K)
air-alarm-ui-window-temperature-indicator = Temperatura: [color={ $color }]{ $tempC } °C ({ $temperature } K)[/color]
air-alarm-ui-window-alarm-state = [color={ $color }]{ $state ->
    [normal] Normal
    [warning] Advertencia
    [danger] Peligro
    [emagged] Saboteada
   *[invalid] No válido
}[/color]
air-alarm-ui-window-alarm-state-indicator = Estado: [color={ $color }]{ $state ->
    [normal] Normal
    [warning] Advertencia
    [danger] Peligro
    [emagged] Saboteada
   *[invalid] No válido
}[/color]

air-alarm-ui-window-tab-vents = Respiraderos
air-alarm-ui-window-tab-scrubbers = Depuradores
air-alarm-ui-window-tab-sensors = Sensores

air-alarm-ui-gases = { $gas }: { $amount } mol ({ $percentage } %)
air-alarm-ui-gases-indicator = { $gas }: [color={ $color }]{ $amount } mol ({ $percentage } %)[/color]

air-alarm-ui-mode-filtering = Filtrado
air-alarm-ui-mode-wide-filtering = Filtrado amplio
air-alarm-ui-mode-fill = Llenado
air-alarm-ui-mode-panic = Pánico
air-alarm-ui-mode-none = Ninguno

air-alarm-ui-pump-direction-siphoning = Sifonado
air-alarm-ui-pump-direction-scrubbing = Depuración
air-alarm-ui-pump-direction-releasing = Liberación

air-alarm-ui-pressure-bound-nobound = Sin límite
air-alarm-ui-pressure-bound-internalbound = Límite interno
air-alarm-ui-pressure-bound-externalbound = Límite externo
air-alarm-ui-pressure-bound-both = Ambos

air-alarm-ui-widget-gas-filters = Filtros de gases

## Controles

### General

air-alarm-ui-widget-enable = Habilitado
air-alarm-ui-widget-copy = Copiar ajustes a dispositivos similares
air-alarm-ui-widget-copy-tooltip = Copia los ajustes de este dispositivo a todos los dispositivos de la pestaña actual de la alarma atmosférica.
air-alarm-ui-widget-ignore = Ignorar
air-alarm-ui-atmos-net-device-label = Dirección: { $address }

### Bombas de los respiraderos

air-alarm-ui-vent-pump-label = Dirección del respiradero
air-alarm-ui-vent-pressure-label = Límite de presión
air-alarm-ui-vent-external-bound-label = Límite externo
air-alarm-ui-vent-internal-bound-label = Límite interno

### Depuradores

air-alarm-ui-scrubber-pump-direction-label = Dirección
air-alarm-ui-scrubber-volume-rate-label = Caudal (L)
air-alarm-ui-scrubber-wide-net-label = Red amplia
air-alarm-ui-scrubber-select-all-gases-label = Seleccionar todos
air-alarm-ui-scrubber-deselect-all-gases-label = Deseleccionar todos

### Umbrales

air-alarm-ui-sensor-gases = Gases
air-alarm-ui-sensor-thresholds = Umbrales
air-alarm-ui-thresholds-pressure-title = Umbrales (kPa)
air-alarm-ui-thresholds-temperature-title = Umbrales (K)
air-alarm-ui-thresholds-gas-title = Umbrales (%)
air-alarm-ui-thresholds-upper-bound = Peligro por encima de
air-alarm-ui-thresholds-lower-bound = Peligro por debajo de
air-alarm-ui-thresholds-upper-warning-bound = Advertencia por encima de
air-alarm-ui-thresholds-lower-warning-bound = Advertencia por debajo de
air-alarm-ui-thresholds-copy = Copiar los umbrales a todos los dispositivos
air-alarm-ui-thresholds-copy-tooltip = Copia los umbrales de los sensores de este dispositivo a todos los dispositivos de la pestaña actual de la alarma atmosférica.
