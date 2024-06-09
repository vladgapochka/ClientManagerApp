// Функция для отображения модального окна создания нового клиента
function showCreateModal() {
    $('#createClientModal').modal('show');
}


// Функция для отправки формы создания клиента и обработки ответа сервера
function submitCreateForm() {
    $.ajax({
        type: 'POST',
        url: '/Clients/Create',
        data: $('#createClientForm').serialize(),
        success: function (response) {
            $('#createClientModal').modal('hide');
            location.reload(); 
        },
        error: function (err) {
            console.log(err);
        }
    });
}

// Функция для редактирования клиента. Загружает данные клиента по ID и отображает их в форме.
function editClient(id) {
    $.ajax({
        type: 'GET',
        url: `/Clients/GetClient/${id}`,
        success: function (data) {
            $('#editId').val(data.id);
            $('#editFirstName').val(data.firstName);
            $('#editLastName').val(data.lastName);
            $('#editEmail').val(data.email);
            $('#editPhone').val(data.phone);
            $('#editClientModal').modal('show');
        },
        error: function (err) {
            console.error("Ошибка при получении данных клиента: ", err);
        }
    });
}


// Функция для отправки отредактированной формы и обновления данных клиента
function submitEditForm() {
    $.ajax({
        type: 'POST',
        url: '/Clients/UpdateClient',
        data: $('#editClientForm').serialize(),
        success: function (response) {
            $('#editClientModal').modal('hide');
            location.reload(); // Перезагрузка страницы для обновления списка клиентов
        },
        error: function (err) {
            console.log(err);
        }
    });
}

// Функция, привязанная к кнопкам удаления каждого клиента в списке
$(document).ready(function () {
    $('.delete-client-btn').on('click', function () {
        var clientId = $(this).data('id');
        if (confirm('Вы уверены, что хотите удалить этого клиента?')) {
            $.ajax({
                url: '/Clients/Delete/' + clientId,
                type: 'POST',
                success: function (result) {
                    window.location.reload(); // Перезагрузить страницу после успешного удаления
                },
                error: function (err) {
                    console.error('Произошла ошибка при удалении клиента: ', err);
                }
            });
        }
    });
});


// Обработчик для закрытия всех модальных окон при клике на элементы с атрибутом data-dismiss="modal"
$(document).ready(function () {
    $('[data-dismiss="modal"]').click(function () {
        $('.modal').modal('hide');
    });
});


