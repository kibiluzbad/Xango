$.ajaxPrefilter(function (options, originalOptions, jqXHR) {
    var verificationToken = $("meta[name='__AjaxRequestVerificationToken']").attr('content');
    if (verificationToken) {
        jqXHR.setRequestHeader("X-Request-Verification-Token", verificationToken);
    }
});