// ReSharper disable once UseOfImplicitGlobalInFunctionScope
$(document).ready(function () {

    $("#Horarios").change(function () {
        $.ajax({
            url: urlBase + "/Chamada/AlteraAgendas?horarioId=" + $("#Horarios").val(),
            type: "GET",
        })
            .done(function (result) {
                $("#divAgendas").html(result);
                bootstrapToggle();
            });
    });

    $("#btnCreate").click(function () {

        var agendasId = $("#AgendasId").val().split(',');

        for (var i = 0; i < agendasId.length; i++) {
            var obj = {};
            obj.agendaId = agendasId[i];
            obj.presenca = $("#itemPresenca" + agendasId[i]).val();

            $.ajax({
                url: urlBase + "/Chamada/Create",
                data: obj,
                type: "POST",
            });
        }

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


