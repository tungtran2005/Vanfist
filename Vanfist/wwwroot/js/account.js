document.addEventListener('DOMContentLoaded', function () {
    const links = document.querySelectorAll('.section-link');
    const sections = document.querySelectorAll('.content-section');

    function showSection(name) {
        sections.forEach(s => s.style.display = 'none');
        const el = document.getElementById(`section-${name}`);
        if (el) el.style.display = '';
    }

    links.forEach(link => {
        link.addEventListener('click', async function (e) {
            e.preventDefault();
            const section = this.getAttribute('data-section');
            showSection(section);

            if (section === "transactions") {
                await loadTransactions();
            }
        });
    });

    showSection('personal');

    const form = document.getElementById('changePasswordForm');
    if (form) {
        const newPwd = form.querySelector('#NewPassword');
        const confirmPwd = form.querySelector('#ConfirmPassword');
        const submitBtn = form.querySelector('#btnChangePasswordSubmit');
        const help = document.getElementById('ConfirmPasswordHelp');

        function validatePasswordsMatch() {
            const match = confirmPwd.value.length > 0 && newPwd.value === confirmPwd.value;
            if (!match) {
                help.classList.remove('d-none');
                confirmPwd.setCustomValidity('Mật khẩu xác nhận không khớp.');
                submitBtn.disabled = true;
            } else {
                help.classList.add('d-none');
                confirmPwd.setCustomValidity('');
                submitBtn.disabled = false;
            }
        }

        newPwd.addEventListener('input', validatePasswordsMatch);
        confirmPwd.addEventListener('input', validatePasswordsMatch);

        form.addEventListener('submit', function (e) {
            validatePasswordsMatch();
            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
                form.reportValidity();
            }
        });

        validatePasswordsMatch();
    }
});

async function loadTransactions(page = 1, pageSize = 5) {
    const container = document.querySelector('#section-transactions .card-body');
    container.innerHTML = `<div class="text-muted">Đang tải dữ liệu...</div>`;

    try {
        const response = await fetch(`/api/InvoiceApi/paged?page=${page}&pageSize=${pageSize}`);
        if (!response.ok) throw new Error("Không thể load dữ liệu");

        const result = await response.json();
        const invoices = result.items || [];

        if (invoices.length === 0) {
            container.innerHTML = `<div class="text-muted">Bạn chưa có đơn hàng nào.</div>`;
            return;
        }

        let tableHtml = `
            <table class="table table-bordered table-sm">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>ModelId</th>
                        <th>TotalPrice</th>
                        <th>Status</th>
                        <th>Type</th>
                        <th>CreatedAt</th>
                    </tr>
                </thead>
                <tbody>
                    ${invoices.map(i => `
                        <tr>
                            <td>${i.id}</td>
                            <td>${i.modelId}</td>
                            <td>${i.totalPrice.toLocaleString()} VND</td>
                            <td>${i.status}</td>
                            <td>${i.type}</td>
                            <td>${new Date(i.createdAt).toLocaleString()}</td>
                        </tr>
                    `).join('')}
                </tbody>
            </table>
        `;

        const totalPages = Math.ceil(result.totalCount / result.pageSize);
        let paginationHtml = `<nav><ul class="pagination justify-content-center mt-3">`;
        for (let p = 1; p <= totalPages; p++) {
            paginationHtml += `
                <li class="page-item ${p === page ? 'active' : ''}">
                    <a class="page-link" href="#" data-page="${p}">${p}</a>
                </li>`;
        }
        paginationHtml += `</ul></nav>`;

        container.innerHTML = tableHtml + paginationHtml;

        container.querySelectorAll('.page-link').forEach(link => {
            link.addEventListener('click', e => {
                e.preventDefault();
                const newPage = parseInt(link.getAttribute('data-page'));
                loadTransactions(newPage, pageSize);
            });
        });

    } catch (err) {
        console.error(err);
        container.innerHTML = `<div class="text-danger">Lỗi khi tải dữ liệu đơn hàng.</div>`;
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const section = document.querySelector('#section-transactions');
    if (section) loadTransactions();
});
