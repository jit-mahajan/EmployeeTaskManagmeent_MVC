document.addEventListener("DOMContentLoaded", function () {

    const employeeDropdown = document.getElementById("EmployeeId");

    if (!employeeDropdown) {
        return;
    }

    employeeDropdown.addEventListener("change", function () {

        const employeeId = this.value;

        if (!employeeId) {

            document.getElementById("employeeName").textContent = "-";
            document.getElementById("employeeEmail").textContent = "-";
            document.getElementById("employeeDepartment").textContent = "-";
            document.getElementById("employeeStatus").textContent = "-";

            return;
        }

        fetch("/Task/GetEmployeeInfo?id=" + employeeId)
            .then(response => {

                if (!response.ok) {
                    throw new Error("Employee not found");
                }

                return response.json();
            })
            .then(employee => {

                document.getElementById("employeeName").textContent =
                    employee.name || "-";

                document.getElementById("employeeEmail").textContent =
                    employee.email || "-";

                document.getElementById("employeeDepartment").textContent =
                    employee.department || "-";

                document.getElementById("employeeStatus").textContent =
                    employee.isActive ? "Active" : "Inactive";
            })
            .catch(error => {

                console.error(error);

                document.getElementById("employeeName").textContent = "-";
                document.getElementById("employeeEmail").textContent = "-";
                document.getElementById("employeeDepartment").textContent = "-";
                document.getElementById("employeeStatus").textContent = "-";
            });
    });

});