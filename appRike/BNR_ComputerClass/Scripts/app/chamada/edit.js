// ReSharper disable once UseOfImplicitGlobalInFunctionScope
var baseUrl = urlBase;

$(document).ready(function () {
    $("#btnEdit").click(function () {
        var erro = false;
        var agendasId = $("#AgendasId").val().split(',');

        for (var i = 0; i < agendasId.length && !erro; i++) {
            var obj = {};
            var agendaId = agendasId[i];
            obj.id = $("#chamadaId" + agendaId).val();
            obj.presenca = $("#itemPresenca" + agendaId).val();

            $.ajax({
                url: baseUrl + "Chamada/Edit",
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
            alert("Ocorreu erros na alteração da chamada!");
        } else {
            window.location = "/Chamada/Index";
        }
    });

    registerChangeCheckbox();
    registerChangeSelect();
    bootstrapToggle();
});

function bootstrapToggle() {
    $(":checkbox").bootstrapToggle({
        on: 'Presente',
        off: 'Faltou',
        offstyle: 'danger'
    });
}

function registerChangeCheckbox() {
    $(":checkbox").change(function () {
        var valor = $("#itemPresenca" + this.id).val().toLowerCase() == "false" ? false : true;
        $("#itemPresenca" + this.id).val(!valor);
    });
}

function registerChangeSelect() {
    $("#Horarios").change(function () {
        $.ajax({
            url: baseUrl + "/Chamada/AtualizaAgendasEdit?horarioId=" + $(this).val() + "&aulaId=" + $("#Id").val(),
            type: "GET",
        })
            .done(function (result) {
                $("#divAgendas").html(result);
                bootstrapToggle();
                registerChangeCheckbox();
                registerChangeSelect();
            });
    });
}