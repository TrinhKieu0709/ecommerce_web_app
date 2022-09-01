import { ajax, data, type } from "jquery";

var contact = {
    init: function () {
        contact.registerEvents();
    },
    registerEvents: function () {
        $('btnsend').off('click').on('click', function () {
            var name = $(' #txtName').val;
            var address = $(' #txtAddress').val;
            var phonenumber = $('#txtMobile').val;
            var mail = $('#txtMail').val;
            var content = $('#txtContent').val;

            $ ajax({
                url: '/Contact/Send',
                type: 'Post',
                dataType: 'json'
                data: {
                    name: name,
                    address: address,
                    phonenumber: phonenumber,
                    mail: mail,
                    content: content,


                },
                success: function (res)
                {
                    if (res.status == true) {
                        alert('Request was sent successfully');

                    }
                }

            });
 
        })
    }

        
}
contact.init();