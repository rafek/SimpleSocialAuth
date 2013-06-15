var providers = {
    google: {
        name: "Google",
        type: 1
    },
    facebook: {
        name: "Facebook",
        type: 2
    },
    twitter: {
        name: "Twitter",
        type: 3
    }
};

var auth = {
    signin: function (providerName) {
        var provider = providers[providerName];

        if (!provider) {
            return;
        }

        $("#authType").val(provider.name);

        $("#authForm").submit();
    }
};