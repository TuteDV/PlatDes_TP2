# Trabajo Práctico 1 de Plataformas de Desarrollo Grupo 1
Integrantes:
- Alberti, Eliana
- Beer, Gustavo
- Da Silva, Franco Ariel
- Guastella, Juan Cruz 
- Manini, Cristian Eduardo


## Definiciones
Para este trabajo implementaremos una aplicación de Windows Form que simula un Home
Banking utilizando C# con .NET Core (Se puede utilizar .NET Framework, pero puede significar
más trabajo al querer realizar el trabajo final de la materia que será una web con MVC y .NET
Core). Se recomienda el uso de Visual Studio 2022 y .NET Core 6.0 para evitar problemas de
compatibilidad
El siguiente diagrama UML muestra las clases involucradas en el modelo, debajo se brindan
mayores detalles del sistema:


## Detalles
- La clase Banco es la principal del programa, ella contiene una lista de Usuarios, Caja de
Ahorro, Tarjeta y Plazo Fijo
- Un Usuario tiene una lista con 0 o más Cajas de Ahorro mientras que la Caja de ahorro
puede tener 1 o más titulares (relación many to many).
- El Usuario tiene una lista de 0 o más Plazo Fijo pero el Plazo Fijo sólo tiene un usuario
titular (relación uno a muchos). Esta misma relación se plantea entre Usuario y Tarjeta
de Crédito y entre Usuario y Pago.
- La Caja de Ahorro guarda un detalle con 0 o más Movimientos que deberían poder
determinar su saldo (relación uno a muchos). Toda operación que involucre una caja
de ahorro, crea un nuevo Movimiento y lo vincula con la Caja de Ahorro
correspondiente, de modo que al visualizar los movimientos sea posible reconstruir el
saldo desde el inicio hasta el momento actual.
- Todas las clases que tienen Get() y Set() en métodos deben implementar getters y
setters para sus elementos. Se recomienda el uso de properties públicas para agilizar
el desarrollo en futuras instancias.
- El diagrama incluye una clase denominada Form que es la interfaz gráfica Windows
Form. La clase Form no realiza NINGUNA acción referida a la lógica. Solo toma los
datos que introduce el usuario, se los pasa a su objeto Banco (que realiza todo el
procesamiento y devuelve resultados) y muestra a los usuarios el resultado de sus
acciones. Debe existir una ÚNICA instancia de banco que se va pasando por referencia
a las distintas pantallas en caso de utilizar MDI.

## Métodos
Los métodos de Alta, Baja y Modificación pueden tomar como parámetro los datos necesarios
para construir el objeto en la clase lógica (Banco) o bien recibir un objeto (en el diagrama se
expresa sólo “ABM Clases” por cuestiones de espacio) y actuar en consecuencia, el diseño
queda a criterio del grupo siempre que respeten los principios de encapsulamiento (la clase
gráfica NO debe modificar los objetos de la clase lógica).
OBS: La clase banco puede contar con cualquier otro atributo privado que consideren
necesario, por ejemplo, puede guardar un UsuarioActual: Usuario que es una referencia al
usuario que inició sesión.

### ABM Clases
- AltaUsuario(in Usuario u): Agrega el usuario u a la lista.
- ModificarUsuario(in Usuario u): Como parámetro obtiene todos los datos del usuario
(al menos aquellos que son modificables, el ID se pasa para identificar el usuario pero
no se puede modificar) y actualiza el que corresponde en la lista.
- EliminarUsuario(): Elimina todos los productos del usuario. Luego elimina el usuario
Usuario.
- AltaCajaAhorro(in Usuario): Agrega una Caja de Ahorro a la lista con saldo 0 y la vincula
con el Usuario.
- BajaCajaAhorro (in ID): Si el saldo de la caja de ahorro es 0, elimina la caja de ahorro de
la lista del usuario y del banco.
- ModificarCajaAhorro (in ID): Modifica datos de la Caja de Ahorro. Permite agregar o
quitar un titular, no puede quedar sin titulares. No se pueden modificar los
movimientos. No se puede modificar el saldo.
- AltaMovimiento (in Movimiento, in Caja): Vincula el movimiento con la caja de ahorro
indicada, agrega el movimiento a la lista de la caja y del banco.
- BajaMovimiento(): No implementar, no está permitido.
- ModificarMovimiento(): No implementar, no está permitido.
- AltaPago(in Pago, in Usuario): Agrega un Pago a la lista y lo vincula con el Usuario.
- BajaPago (in ID): Da de baja el pago, no es lo mismo que pagar el pago, en dicho caso
el pago sigue existiendo en estado “pagado”, esta acción elimina el registro del pago.
Elimina el pago de la lista del usuario y del banco.
- ModificarPago (in ID): Modifica Datos del pago. Permite pasar a estado pagado.
- AltaPlazoFijo(in Plazo Fijo, in Usuario): Agrega un Plazo Fijo a la lista y lo vincula con el
Usuario.
- BajaPlazoFijo (in ID): Sólo si el estado es pagado y la fecha actual es 1 mes posterior a
la fecha de vencimiento permite operar, esta acción elimina el registro del Plazo Fijo,
no es lo mismo que pagar. Elimina el Plazo Fijo de la lista del usuario y del banco.
- ModificarPlazoFijo (in ID): NO implementar, no está permitido modificar un Plazo Fijo.
- AltaTarjetaCredito(in Tarjeta de Crédito, Usuario) Crea una nueva tarjeta de crédito
con los datos ingresados, la vincula con el usuario y la agrega a la lista de tarjetas del
banco.
- BajaTarjetaCredito (in ID): Elimina la tarjeta de crédito sólo si la misma contiene $0 en
consumos. Quita la tarjeta de la lista del usuario y del banco.
- ModificarTarjetaCredito (in ID): Solo permite modificar el límite

### Mostrar Datos
- MostrarCajasDeAhorro(): Devuelve una lista con las cajas de ahorro del usuario actual
logueado.
- MostrarMovimientos (in Caja de Ahorro): Devuelve una lista con los movimientos de la
caja de ahorro ingresada.
- MostrarPagos(): Devuelve una lista con los pagos del usuario actual logueado.
- MostrarPlazoFijos(): Devuelve una lista con los plazo fijos del usuario actual logueado.
- MostrarTarjetasDeCredito(): Devuelve una lista con las tarjetas de crédito del usuario
actual logueado.
OBS: Los métodos VOID (que no devuelven nada) de ABM, bien pueden devolver un bool para
indicar a la interfaz si las acciones se realizaron correctamente o no y en base a ello mostrar el
cartel que corresponda.

### Operaciones Usuario
- IniciarSesion(in string Usuario, in string Contraseña): Busca en la lista de usuarios un
usuario que tenga ese identificador (a criterio del grupo, pueden usar DNI o mail, por
ejemplo) y esa contraseña. Si es correcto y no está bloqueado devuelve TRUE, se
recomienda guardar el usuario logueado (UsuarioActual) en la clase Banco ya que esto
nos facilitará las cosas en métodos subsiguientes. Caso contrario agrega un intento
fallido al usuario, si tiene 3 intentos pasa a estado bloqueado, y devuelve FALSE.
- CerrarSesion():UsuarioActual pasa a ser nulo.
- CrearCajaAhorro(): Crea una nueva Caja de Ahorro con los datos ingresados.
- Depositar (in Caja de Ahorro, float monto): Deposita el monto ingresado en la Caja de
Ahorro seleccionada.
- Retirar (in Caja de Ahorro, float monto): Retira el monto ingresado en la Caja de
Ahorro seleccionada siempre que cuente con saldo suficiente, caso contrario devuelve
un error.
- Transferir (in Caja de Ahorro origen, in Caja de Ahorro destino, float Monto):
Transfiere el monto indicado de la caja de ahorro seleccionada del usuario actual
(origen) a otra caja de ahorro (destino, que puede ser del usuario actual o no), siempre
que el saldo de origen sea suficiente para realizar la operación caso contrario genera
un error.
- BuscarMovimiento(in Caja de Ahorro, in string Detalle, in date Fecha, in float Monto):
Devuelve una lista de movimientos de la caja de ahorro filtrada por al menos uno de
los parámetros (el usuario puede ingresar los 3 parámetros, 2 o 1) Detalle, Fecha y
Monto.
- PagarTarjeta(in Tarjeta, in Caja de Ahorro): Descuenta el saldo total de consumos de la
tarjeta del saldo de la caja de ahorro (generando el movimiento correspondiente)
siempre que el saldo sea suficiente, caso contrario, no permite operar.


## Condiciones de aprobación

Se deben implementar las distintas vistas en el programa Windows Form siguiendo el siguiente
esquema:
1. Desarrollar todas las clases propuestas en el esquema principal (incluyendo getters y
setters en caso que corresponda) y todos los métodos detallados.
2. Al ejecutar, el usuario observa una pantalla de inicio de sesión.
a. Existe un botón que permite registrarse, esto lleva al usuario a una nueva
pantalla donde se piden sus datos y puede crear una nueva cuenta.
b. Si el usuario es válido pero la clave es incorrecta, se suma un intento fallido.
c. Si el usuario llega a 3 intentos fallidos, el mismo queda bloqueado y no puede
acceder, aunque coloque la clave correcta.
3. Si las credenciales son correctas, el usuario ingresa a la pantalla principal donde
observa:
a. Caja de Ahorro: Al hacer clic se abre una ventana con el listado de sus cajas de
ahorro y la opción para crear una nueva caja. Al seleccionar una caja se
habilitan las opciones de baja y modificación. También las operaciones
Depositar, Retirar, Transferir y ver detalle que muestra los movimientos de la
caja. Al visualizar los movimientos de la caja, es posible filtrar por Detalle,
Fecha o Monto. Toda operación que involucre una Caja de Ahorro crea un
nuevo movimiento que queda asociada a la misma.
b. Pagos: Al hacer clic se abre una ventana con el listado de pagos asociados al
usuario, divididos en dos tablas, pagos realizados y pagos pendientes. También
se permite la opción para crear un nuevo Pago. Al seleccionar un pago se
habilitan las opciones de eliminar (sólo si fue pagado) o pagar (si no fue
pagado). Para pagar el pago, se le permite al usuario elegir en una lista de
string donde figuran sus cuentas (CBU) y sus tarjetas (número). Al seleccionar
pagar se verifica que el instrumento seleccionado tenga saldo/límite
disponible según corresponda, si es así se descuenta el pago del medio y se
genera un movimiento si es Caja de Ahorro o se aumentan los consumos si es
tarjeta. De lo contrario, no se permite operar por falta de saldo.
c. Plazo Fijo: Al hacer clic se abre una ventana con el listado de sus plazos fijos y
la opción para crear uno nuevo. Al crear un nuevo Plazo Fijo se selecciona la
cuenta de la cual se desea retirar el dinero para constituirlo, si el saldo es
insuficiente no permite operar. También se valida que el monto sea mínimo
$1.000 para poder crearlo. La tasa es un parámetro fijado por el programa, el
usuario la visualiza, pero no puede editarla. Al seleccionar un plazo fijo, se
puede eliminar del listado si el mismo ya está pagado y pasó más de un mes
desde la FechaFin.
OBS: Al iniciar, el sistema DEBE recorrer la lista de plazo fijos, si alguno tiene
FechaFin igual a la fecha de hoy, entonces debe proceder a depositar el monto
final, calcular monto ingresado + interés (monto * (tasa / 365) * cantidad de
días) y marcar el plazo fijo como pagado.
d. Tarjetas de Crédito: Al hacer clic se abre una ventana con el listado de sus
Tarjetas de crédito y la opción para crear una nueva. Al seleccionar una tarjeta,
se habilitan las opciones para darla de baja o pagar sus consumos.
4. Se muestran los carteles que corresponda para cada operación, informando el
resultado de la ejecución de la misma.
5. No Incluir lógica en la clase gráfica Form, esta regla es condición de aprobación.

## Condiciones de entrega
- La solución completa con todo el código se debe entregar en un archivo .zip con
nombre TP1 – Grupo X. Reemplazar X por identificador del grupo (nombre o número).
- Se debe incluir dentro del .zip un archivo “ReadMe” que explique cómo utilizar ambos
programas y cualquier aclaración que consideren necesaria o decisión de diseño que
hayan tomado (puede ser en formato Word, txt, Excel, etc. No se evalúa presentación,
pero si contenido del mismo).
- La entrega se debe realizar por mail a mi casilla: walter.gomez@davinci.edu.ar
- El sujeto debe ser: Plataformas de programación: TP1 – Entrega grupo X. Reemplazar X
por identificador del grupo (nombre o número).
- El cuerpo debe contener el detalle de los alumnos que componen el grupo, uno solo
de ellos hace la entrega por el grupo pero debe poner en copia al resto, siendo
responsabilidad de todos los integrantes la entrega del trabajo.
- La fecha de entrega está establecida en Classroom. Entregar fuera de término afecta
sobre la nota del trabajo práctico.
- Tengan en cuenta que la entrega se puede complicar ya que el mail no acepta archivos
.EXE aun si los mismos están dentro de otro archivo .zip. También pueden compartir su
proyecto por drive o enviarme el link de un repositorio.
- El proyecto debe compilar tal como fue entregado, caso contrario, será desaprobado.
