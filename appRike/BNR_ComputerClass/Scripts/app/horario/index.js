// ReSharper disable once UseOfImplicitGlobalInFunctionScope

$(document).ready(function () {
    var baseUrl = urlBase;

    registerChangeSelect();

    function registerChangeSelect() {
        $("#Dia").change(function () {
            $.ajax({
                url: baseUrl + "/Horario/AtualizaHorariosIndex?dia="+ $(this).val(),
                type: "GET",
            })
            .done(function (result) {
                $("#divAgendas").html(result);
                registerChangeSelect();
            });
        });

        $("#Horarios").change(function () {
            $.ajax({
                url: baseUrl + "/Horario/AtualizaAgendasIndex?horarioId=" + $(this).val(),
                type: "GET",
            })
            .done(function (result) {
                $("#divAgendas").html(result);
                registerChangeSelect();
            });
        });
    }
});




