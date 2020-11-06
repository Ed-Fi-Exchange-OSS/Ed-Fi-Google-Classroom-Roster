function UpdateOptions(url, data, targetselect, emptySelection)
{
    $.ajax({
        url: url,
        type: 'GET',
        datatype: 'JSON',
        data: data,
        success: function (result) {
            console.log(result);
            console.log($(targetselect).html());
            $(targetselect).html('');
            $(targetselect).append($('<option value="">' + emptySelection + '</option>'));
            $.each(result, function (i, item) {
                $(targetselect).append($('<option></option>').val(item.Value).html(item.Text));
            });
        }
    });
}

