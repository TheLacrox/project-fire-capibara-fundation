### Interfaz

# Se muestra en la ventana de preferencias del personaje.
humanoid-character-profile-summary =
    { $gender ->
        [male] Este es { $name }
        [female] Esta es { $name }
        [epicene] Esta persona es { $name }
       *[other] Este ser es { $name }
    } y tiene { $age } años.
