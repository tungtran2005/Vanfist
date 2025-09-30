document.addEventListener('DOMContentLoaded', function () {
    const links = document.querySelectorAll('.section-link');
    const sections = document.querySelectorAll('.content-section');

    // Hiển thị section theo tên
    function showSection(name) {
        sections.forEach(s => s.style.display = 'none');
        const el = document.getElementById(`section-${name}`);
        if (el) el.style.display = '';
    }

    // Lắng nghe click menu
    links.forEach(link => {
        link.addEventListener('click', async function (e) {
            e.preventDefault();
            const section = this.getAttribute('data-section');
            showSection(section);

            // Nếu mở Transactions thì load dữ liệu
            if (section === "transactions") {
                await loadTransactions();
            }
        });
    });

    // Default mở phần thông tin cá nhân
    showSection('personal');

    // --- Validate đổi mật khẩu ---
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

        // Khởi tạo trạng thái nút submit
        validatePasswordsMatch();
    }
});

// --- Hàm load invoice (transactions) ---
async function loadTransactions() {
    const accountIdEl = document.getElementById('accountId');
    if (!accountIdEl) return;

    const accountId = accountIdEl.value;
    const container = document.querySelector('#section-transactions .card-body');
    container.innerHTML = `<div class="text-muted">Đang tải dữ liệu...</div>`;

    try {
        const response = await fetch(`/api/InvoiceApi/account/${accountId}`);
        if (!response.ok) throw new Error("Không thể load dữ liệu");
        const invoices = await response.json();

        if (!invoices || invoices.length === 0) {
            container.innerHTML = `<div class="text-muted">Bạn chưa có đơn hàng nào.</div>`;
        } else {
            container.innerHTML = `
                <table class="table table-bordered table-sm">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>AccountId</th>
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
                                <td>${i.accountId}</td>
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
        }
    } catch (err) {
        console.error(err);
        container.innerHTML = `<div class="text-danger">Lỗi khi tải dữ liệu đơn hàng.</div>`;
    }
}
