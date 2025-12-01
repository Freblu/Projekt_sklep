# Admin Dashboard

## Overview

Administrator dashboard został zaimplementowany na podstawie projektu z Figmy, zapewniając kompleksowe narzędzie do zarządzania platformą e-commerce.

## Funkcjonalności

### 1. Dashboard Overview (`/Admin/Index`)
- **Statystyki ogólne**: 
  - Total Products
  - Total Orders
  - Total Customers
  - Total Revenue (z procentowym wzrostem)
- **Alerty**:
  - Low Stock Alert - produkty o niskim stanie magazynowym
  - Out of Stock - produkty niedostępne
- **Recent Products** - najnowsze produkty z możliwością edycji i usuwania
- **Recent Orders** - ostatnie zamówienia z statusami
- **Quick Stats** - szybkie statystyki (średnia wartość zamówienia, zamówienia dzisiaj, nowi klienci)

### 2. Products Management (`/Admin/Products`)
- **Lista produktów** z możliwością:
  - Filtrowania po kategorii
  - Wyszukiwania po nazwie
  - Paginacji wyników
- **Informacje o produktach**:
  - Zdjęcie produktu
  - Nazwa i ID
  - Kategoria
  - Cena
  - Stan magazynowy (z kolorowym oznaczeniem: OK, Low Stock, Out of Stock)
  - Status (Active, Draft, Inactive)
  - Data utworzenia
- **Akcje**:
  - Edycja produktu
  - Usuwanie produktu (z potwierdzeniem)

### 3. Add/Edit Product (`/Admin/ProductCreate`, `/Admin/ProductEdit`)
- **Formularz produktu**:
  - Nazwa produktu
  - Opis
  - Cena
  - Ilość w magazynie
  - Kategoria (wybór z listy)
  - Marka
  - Zdjęcie (URL lub upload pliku)
  - Status (Active, Draft, Inactive)
- **Podgląd zdjęcia** - automatyczny podgląd uploadowanego zdjęcia
- **Walidacja** - formularz z walidacją po stronie klienta i serwera
- **Zabezpieczenia** - antiforgery tokens

## Struktura Plików

```
PartsCom.Ui/
├── Controllers/
│   └── AdminController.cs          # Kontroler administratora
├── Models/
│   └── AdminDashboardViewModel.cs  # Modele danych dla dashboardu
├── Views/
│   └── Admin/
│       ├── Index.cshtml            # Dashboard overview
│       ├── Products.cshtml         # Lista produktów
│       ├── ProductCreate.cshtml    # Dodawanie produktu
│       └── ProductEdit.cshtml      # Edycja produktu
└── wwwroot/
    └── css/
        └── dashboard/
            └── admin-dashboard.css # Style dla dashboardu
```

## Dostęp do Dashboardu

Dashboard administratora jest dostępny pod następującymi URL:
- `/Admin/Index` - Dashboard overview
- `/Admin/Products` - Zarządzanie produktami
- `/Admin/ProductCreate` - Dodawanie produktu
- `/Admin/ProductEdit/{id}` - Edycja produktu
- `/Admin/Orders` - Zarządzanie zamówieniami (placeholder)
- `/Admin/Customers` - Zarządzanie klientami (placeholder)
- `/Admin/Settings` - Ustawienia (placeholder)

## Design

Dashboard został zaprojektowany w oparciu o:
- **Figma Design**: https://www.figma.com/design/sjrTFszzm54C6n0bYdv3Uv/Clicon---eCommerce-Marketplace-Website-Figma-Template--Community---Community-
- **Inspiracja**: User dashboard (`user-dashboard.png`)
- **Styl**: Nowoczesny, minimalistyczny interfejs z wyraźnymi ikonami i kolorowym kodowaniem statusów

## Kolory i Ikony

- **Primary (Blue)**: #2DA5F3 - główne akcje, aktywne elementy
- **Success (Green)**: #2DB224 - pozytywne statusy, produkty dostępne
- **Warning (Orange)**: #FA8232 - ostrzeżenia, niski stan magazynowy
- **Danger (Red)**: #EE4F3E - błędy, brak produktów
- **Purple**: #8B5CF6 - dodatkowe akcenty
- **SVG Icons**: Feather Icons style - minimalistyczne, liniowe ikony

## Responsywność

Dashboard jest w pełni responsywny:
- **Desktop** (>1200px): Pełny layout z sidebar
- **Tablet** (768-1199px): Zredukowany sidebar, mniejsze karty
- **Mobile** (<768px): Hamburger menu, pionowy layout, karty na całą szerokość

## Bezpieczeństwo

- **Authentication Required**: Wszystkie akcje wymagają uwierzytelnienia (`[RequireAuthentication]`)
- **Antiforgery Tokens**: Wszystkie formularze zawierają tokeny antiforgery
- **Confirmation Dialogs**: Usuwanie produktów wymaga potwierdzenia

## Funkcje do Rozszerzenia

1. **Orders Management**: Pełne zarządzanie zamówieniami
2. **Customers Management**: Zarządzanie kontami klientów
3. **Analytics**: Zaawansowane wykresy i raporty
4. **Settings**: Konfiguracja platformy
5. **Image Upload**: Integracja z serwisem przechowywania plików
6. **Real-time Updates**: WebSocket dla live updates
7. **Bulk Actions**: Masowe operacje na produktach
8. **Export/Import**: Eksport i import danych (CSV, Excel)

## Technologie

- **ASP.NET Core MVC** - framework
- **Razor Views** - templating
- **CSS3** - stylowanie
- **JavaScript** - interaktywność (preview zdjęć)
- **Bootstrap Grid** - system layoutu

## Mockowe Dane

Obecnie dashboard używa mockowych danych. W wersji produkcyjnej należy:
1. Połączyć z bazą danych (Entity Framework Core)
2. Zaimplementować repository pattern
3. Dodać usługi biznesowe
4. Zaimplementować logikę uploadowania plików
5. Dodać cache dla lepszej wydajności
