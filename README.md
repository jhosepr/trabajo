Control de Inventario (vencidos, por vencer, bajo stock)
Dashboard con métricas
Reportes (medicamentos vencidos, por vencer, inventario completo, bajo stock)
Protección de rutas por rol ([Authorize])
Verificacion y correcta Implementacion del Dashboard y Reportes
=======
## 4. ⚠️ Control de Inventario Inteligente
**Responsable:** `NIHAHT`

El sistema debe calcular y mostrar automáticamente el estado del inventario:

| Estado | Criterio |
|--------|----------|
| 🔴 Vencido | Fecha de vencimiento ya superada |
| 🟡 Por vencer pronto | Vence en 30, 60 o 90 días |
| 🟠 Bajo stock | Cantidad por debajo del umbral mínimo |

> Los estados deben reflejarse visualmente en una **nueva vista** mediante colores o etiquetas distintivas.

---

## 5. 📊 Dashboard de Toma de Decisiones
**Responsable:** `JHOSEP`

Panel principal accesible según rol (**administrador**). Debe mostrar los siguientes indicadores:

- Total de medicamentos en inventario
- Medicamentos vencidos
- Medicamentos próximos a vencer
- Productos con bajo stock
- Total de categorías registradas
- Total de estantes registrados

---

## 🎨 Interfaz y Experiencia (UI/UX)
**Responsables:** `NIHAHT` + `JHOSEP`

Requisitos generales de interfaz para todo el sistema:

- Framework: **Bootstrap** — interfaces en español, limpias y profesionales
- Tablas dinámicas con búsqueda y paginación
- Mensajes de retroalimentación claros (ej: *"Registro creado correctamente"*)
- Confirmación obligatoria antes de eliminar cualquier registro

## 🔀 Merge y Compatibilidad
**Responsable:** `NIHAHT`

Gestionar la integración del código entre ramas y garantizar compatibilidad entre los módulos desarrollados por cada integrante.

---

## 6. 📁 Reportes
**Responsable:** `DANILO`

Generación de reportes en formato **Excel**, **PDF** y con **gráficos** para los siguientes casos:

- Medicamentos vencidos
- Medicamentos por vencer (próximos 30 días)
- Inventario completo
- Productos con bajo stock

---

