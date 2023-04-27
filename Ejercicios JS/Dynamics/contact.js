// Muestra un mensaje en la consola del navegador al cargar el formulario
function onLoadForm(executionContext) {
    debugger;
    const formContext = executionContext.getFormContext();

    console.log("hello form! ", formContext)
}

function onChangePhone(executionContext) {
    debugger;

    onPhoneChangeComprobar9Chars(executionContext);
    onPhoneChangeQuitarNoNumeric(executionContext);
}

// Va quitando los caracteres que no sean numeros del campo telefono
function onPhoneChangeQuitarNoNumeric(executionContext) {
    const formContext = executionContext.getFormContext();

    const phone = formContext.getAttribute("telephone1").getValue();

    // quitar lo que no sea numero
    const phoneClean = phone.replace(/\D/g, "");

    // poner el numero limpio en el campo
    formContext.getAttribute("telephone1").setValue(phoneClean);
}

// Ir comprobando si el campo telefono tiene 9 caracteres
function onPhoneChangeComprobar9Chars(executionContext) {
    const formContext = executionContext.getFormContext();

    const phone = formContext.getAttribute("telephone1").getValue();

    // si tiene 9 caracteres, poner el campo en verde, si no, en rojo
    if (phone.length === 9) {
        formContext.getControl("telephone1").setNotification("El telefono tiene la longitud ideal", "phone");
    } else {
        formContext.getControl("telephone1").setNotification("El teléfono debe tener 9 caracteres", "phone");
    }
}

function createAccountRibbon(formContext) {
    const accountName = formContext.getAttribute("firstname").getValue() ?? "Anónimo";;
    const accountLastName = formContext.getAttribute("lastname").getValue() ?? "Anónimo";;

    const fullName = `${accountName} ${accountLastName}`;

    const account = {
        name: fullName,
    };

    Xrm.WebApi.createRecord("account", account).then(
        function success(result) {
            let lookupValue = new Array();
            let entityname = "account";
            let entityId = result.id;

            lookupValue[0] = new Object();
            lookupValue[0].id = entityId;
            lookupValue[0].name = accountName;
            lookupValue[0].entityType = entityname;

            Xrm.Page.getAttribute("parentcustomerid").setValue(lookupValue);

            console.log("Account created with ID: " + result.id);
        },
        function (error) {
            console.log(error.message);
        }
    );
}