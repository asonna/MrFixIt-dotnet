var test = "it works";

$(function () {
    $(".claimjob").click (function () {
        var jobid = $(this).siblings('.ThisJobId').val();
        var username = $('.ThisUserName-' + jobid).val();
        $(".HideAfterClick-"+jobid).hide();
        $.ajax({
            url: "/Jobs/Claim",
            data: { jobId:jobid, userName: username },
            type: 'GET',
            success: function (result) {
                $('.ClaimedJob-'+jodib).html(result);
        }
    });
});

//$(".claimJob").submit(function (event) {
//    event.preventDefault();
//    $("hideButton").hide();
//    $.ajax({
//        url: "/Jobs/Claim",
//        url: '@url.Action("Claim")',
//        type: 'POST',
//        success: function (result) {
//            $('#ClaimedJob').html(result);
//        }
//    });
//});

//$('.AddButton').click(function () {
//    $('.hideButton').show();
//});