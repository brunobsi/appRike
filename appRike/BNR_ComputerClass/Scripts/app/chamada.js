// ReSharper disable once UseOfImplicitGlobalInFunctionScope
$(document).ready(function () {
    var baseUrl = urlBase;

    $("#Horarios").change(function () {
        $.ajax({
            url: baseUrl + "/Chamada/AlteraAgendas?horarioId=" + $("#Horarios").val(),
            type: "GET",
        })
            .done(function (result) {
                $("#divAgendas").html(result);
                bootstrapToggle();
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
                url: baseUrl + "/Chamada/Create",
                data: obj,
                type: "POST",
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
            window.location = baseUrl + "/Chamada/Index";
        }
        

    });

    $(":checkbox").change(function () {
        var valor = $("#" + this.id).val() == "false" ? false : true;
        $("#" + this.id).val(!valor);
    });

    bootstrapToggle();
});

function bootstrapToggle() {
    $(":checkbox").bootstrapToggle({
        on: 'Presente',
        off: 'Faltou',
        offstyle: 'danger'
    });
}


