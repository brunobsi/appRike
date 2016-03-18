// ReSharper disable once UseOfImplicitGlobalInFunctionScope
$(document).ready(function () {
    var baseUrl = urlBase;

    $("#Horarios").change(function () {
        $.ajax({
            url: baseUrl + "/Chamada/AtualizaAgendasCreate?horarioId=" + $("#Horarios").val(),
            type: "GET",
        })
            .done(function (result) {
                $("#divAgendas").html(result);
                bootstrapToggle();
                changeCheckbox();
            });
    });

    $("#btnCreate").click(function () {
        var erro = false;
        var agendasId = $("#AgendasId").val().split(',');

        for (var i = 0; i < agendasId.length && !erro; i++) {
            var obj = {};
            obj.agendaId = agendasId[i];
            obj.presenca = $("#itemPresenca" + agendasId[i]).val();

            $.ajax({
                url: baseUrl + "Chamada/Create",
                data: obj,
                type: "POST",
                async: false
            })
            .done(function (result) {
                if (!result) {
                    erro = true;
                }
            });
        }

        if (erro) {
            alert("Ocorreu erros na gravação da chamada!");
        } else {
            window.location = "Index";
        }
    });

    bootstrapToggle();
    changeCheckbox();
});

function bootstrapToggle() {
    $(":checkbox").bootstrapToggle({
        on: 'Presente',
        off: 'Faltou',
        offstyle: 'danger'
    });
}

function changeCheckbox() {
    $(":checkbox").change(function () {
        var valor = $("#itemPresenca" + this.id).val().toLowerCase() == "false" ? false : true;
        $("#itemPresenca" + this.id).val(!valor);
    });
}


