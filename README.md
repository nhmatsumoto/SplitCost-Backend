## Arquitetura em Camadas

```mermaid
graph TD
    API[API] --> Application[Application]
    API --> Infrastructure[Infrastructure]
    Application --> Domain[Domain]
    Infrastructure --> Application
    Infrastructure --> Domain

    style API fill:#f9f,stroke:#333,stroke-width:2px
    style Application fill:#bbf,stroke:#333,stroke-width:2px
    style Domain fill:#bfb,stroke:#333,stroke-width:2px
    style Infrastructure fill:#ffb,stroke:#333,stroke-width:2px
