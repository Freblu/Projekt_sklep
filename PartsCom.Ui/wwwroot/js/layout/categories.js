document.addEventListener("DOMContentLoaded", function () {
  const categoriesData = [
    {
      name: "Computer & Laptop",
      subcategories: [
        "All",
        "Gaming Laptop",
        "Office Laptop",
        "Desktop PC",
        "Monitors",
      ],
    },
    {
      name: "Computer Accessories",
      subcategories: ["All", "Keyboards", "Mice", "Headsets", "Webcams"],
    },
    {
      name: "SmartPhone",
      subcategories: [
        "All",
        "iPhone",
        "Samsung",
        "Realme",
        "Xiaomi",
        "Oppo",
        "Vivo",
        "OnePlus",
        "Huawei",
        "Infinix",
        "Tecno",
      ],
    },
    { name: "Headphone" },
    { name: "Mobile Accessories" },
    { name: "Gaming Console" },
    { name: "Camera & Photo" },
    { name: "TV & Homes Appliances" },
    { name: "Watches & Accessories" },
    { name: "GPS & Navigation" },
    { name: "Wearable Technology" },
  ];

  const categoryList = document.getElementById("categoryList");
  const subcategoryList = document.getElementById("subcategoryList");
  const categoriesButton = document.getElementById("categoriesButton");
  const categoriesDropdown = document.getElementById("categoriesDropdown");

  if (
    !categoryList ||
    !subcategoryList ||
    !categoriesButton ||
    !categoriesDropdown
  ) {
    return;
  }

  // ===== budowanie lewej listy kategorii =====
  categoriesData.forEach((category) => {
    const li = document.createElement("li");
    li.classList.add("category-item");
    li.textContent = category.name;

    if (category.subcategories && category.subcategories.length) {
      const arrow = document.createElement("span");
      arrow.classList.add("category-arrow");
      arrow.innerHTML = "&rsaquo;";
      li.appendChild(arrow);
    }

    // desktop – najechanie myszką
    li.addEventListener("mouseenter", () => {
      showSubcategories(category, li);
    });

    // mobile – kliknięcie
    li.addEventListener("click", (e) => {
      if (window.innerWidth < 992) {
        e.stopPropagation();
        showSubcategories(category, li);
      }
    });

    categoryList.appendChild(li);
  });

  function showSubcategories(category, liElement) {
    // zaznaczenie aktywnej kategorii
    document.querySelectorAll(".category-item").forEach((item) => {
      item.classList.remove("active");
    });
    if (liElement) {
      liElement.classList.add("active");
    }

    // jeśli brak subkategorii – chowamy prawą kolumnę
    if (!category.subcategories || category.subcategories.length === 0) {
      subcategoryList.style.display = "none";
      subcategoryList.innerHTML = "";
      return;
    }

    subcategoryList.innerHTML = "";

    category.subcategories.forEach((sub) => {
      const li = document.createElement("li");
      const a = document.createElement("a");

      a.textContent = sub;
      a.href = "#"; // TODO: tu wstaw docelowy link (np. /Products?category=SmartPhone&brand=Samsung)

      li.appendChild(a);
      subcategoryList.appendChild(li);
    });

    subcategoryList.style.display = "block";
  }

  // ===== otwieranie / zamykanie dropdownu =====
  function openDropdown() {
    categoriesDropdown.style.display = "block";
  }

  function closeDropdown() {
    categoriesDropdown.style.display = "none";
    document.querySelectorAll(".category-item").forEach((item) => {
      item.classList.remove("active");
    });
    subcategoryList.style.display = "none";
  }

  // desktop – otwarcie po najechaniu
  categoriesButton.addEventListener("mouseenter", openDropdown);
  categoriesButton.addEventListener("mouseleave", function (e) {
    const related = e.relatedTarget;
    if (
      !categoriesButton.contains(related) &&
      !categoriesDropdown.contains(related)
    ) {
      closeDropdown();
    }
  });

  // mobile – klik zamiast hover
  categoriesButton.addEventListener("click", function (e) {
    if (window.innerWidth < 992) {
      e.preventDefault();
      e.stopPropagation();
      const isVisible = categoriesDropdown.style.display === "block";
      if (isVisible) {
        closeDropdown();
      } else {
        openDropdown();
      }
    }
  });

  // klik poza dropdownem zamyka go
  document.addEventListener("click", function (e) {
    if (!categoriesButton.contains(e.target)) {
      closeDropdown();
    }
  });

  // domyślnie pokaż subkategorie pierwszej kategorii posiadającej dzieci
  const firstWithChildren = categoriesData.find(
    (c) => c.subcategories && c.subcategories.length,
  );
  if (firstWithChildren) {
    const firstLi = categoryList.querySelector(".category-item");
    showSubcategories(firstWithChildren, firstLi);
  }
});
