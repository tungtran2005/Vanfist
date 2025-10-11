document.addEventListener('DOMContentLoaded', function () {
    const links = document.querySelectorAll('.section-link');
    const sections = document.querySelectorAll('.content-section');

    function showSection(name) {
        sections.forEach(s => s.style.display = 'none');
        const el = document.getElementById(`section-${name}`);
        if (el) {
            el.style.display = '';
            if (name === 'orders') {
                loadTransactions();
            }
        }
    }

    links.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const section = this.getAttribute('data-section');
            showSection(section);
        });
    });

    showSection('users');
});


async function loadTransactions(page = 1, pageSize = 5) {
    const container = document.querySelector('#section-orders .card-body');
    if (!container) return;

    container.innerHTML = `<div class="text-muted text-center py-3">Đang tải dữ liệu...</div>`;

    try {
        const response = await fetch(`/api/InvoiceApi/paged?page=${page}&pageSize=${pageSize}`);
        if (!response.ok) throw new Error("Không thể load dữ liệu từ API");

        const result = await response.json();
        const invoices = result.items || [];

        if (invoices.length === 0) {
            container.innerHTML = `<div class="text-muted text-center py-3">Bạn chưa có đơn hàng nào.</div>`;
            return;
        }

        let tableHtml = `
            <table class="table table-bordered table-sm align-middle">
                <thead class="table-light">
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
                            <td>${i.modelId ?? '-'}</td>
                            <td>${(i.totalPrice || 0).toLocaleString()} VND</td>
                            <td>${i.status ?? '-'}</td>
                            <td>${i.type ?? '-'}</td>
                            <td>${new Date(i.createdAt).toLocaleString()}</td>
                        </tr>
                    `).join('')}
                </tbody>
            </table>
        `;

        const totalPages = Math.ceil(result.totalCount / result.pageSize);
        let paginationHtml = `
            <nav>
                <ul class="pagination justify-content-center mt-3">
                    ${Array.from({ length: totalPages }, (_, i) => i + 1)
                .map(p => `
                            <li class="page-item ${p === page ? 'active' : ''}">
                                <a class="page-link" href="#" data-page="${p}">${p}</a>
                            </li>
                        `).join('')}
                </ul>
            </nav>
        `;

        container.innerHTML = tableHtml + paginationHtml;


        container.querySelectorAll('.page-link').forEach(link => {
            link.addEventListener('click', e => {
                e.preventDefault();
                const newPage = parseInt(link.getAttribute('data-page'));
                if (!isNaN(newPage)) loadTransactions(newPage, pageSize);
            });
        });

    } catch (err) {
        console.error(err);
        container.innerHTML = `<div class="text-danger text-center py-3">Lỗi khi tải dữ liệu đơn hàng.</div>`;
    }
}
