namespace Kursplassen.Api.Endpoints;

public static class RootEndpoints
{
    private const string LandingPageHtml = """
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1" />
            <meta name="description" content="Kursplassen API - documentation and service health." />
            <title>Kursplassen API</title>
            <style>
                :root {
                    color-scheme: dark;
                    --bg: #0b1120;
                    --surface: #111827;
                    --border: #1f2937;
                    --text: #e5e7eb;
                    --muted: #94a3b8;
                    --accent: #38bdf8;
                    --accent-strong: #0ea5e9;
                    --success: #34d399;
                }

                * {
                    box-sizing: border-box;
                }

                body {
                    margin: 0;
                    min-height: 100dvh;
                    display: grid;
                    place-items: center;
                    padding: 24px;
                    background:
                        radial-gradient(circle at 15% 15%, rgba(56, 189, 248, 0.12), transparent 45%),
                        radial-gradient(circle at 85% 85%, rgba(129, 140, 248, 0.12), transparent 45%),
                        var(--bg);
                    color: var(--text);
                    font-family: ui-sans-serif, system-ui, -apple-system, "Segoe UI", Roboto, Arial, sans-serif;
                    line-height: 1.5;
                }

                main {
                    width: min(680px, 100%);
                }

                .card {
                    background: color-mix(in srgb, var(--surface) 88%, transparent);
                    border: 1px solid var(--border);
                    border-radius: 20px;
                    padding: 36px;
                    box-shadow: 0 24px 60px rgba(0, 0, 0, 0.35);
                    backdrop-filter: blur(12px);
                }

                .brand {
                    display: flex;
                    align-items: center;
                    gap: 14px;
                    margin-bottom: 10px;
                }

                .logo {
                    display: grid;
                    place-items: center;
                    width: 46px;
                    height: 46px;
                    border-radius: 14px;
                    background: linear-gradient(135deg, var(--accent), #818cf8);
                    color: #04121f;
                    font-weight: 800;
                    font-size: 20px;
                }

                h1 {
                    margin: 0;
                    font-size: clamp(1.7rem, 4vw, 2.15rem);
                    letter-spacing: -0.03em;
                }

                .subtitle {
                    margin: 14px 0 28px;
                    color: var(--muted);
                    font-size: 1rem;
                }

                .links {
                    display: grid;
                    gap: 12px;
                }

                .link {
                    display: flex;
                    align-items: center;
                    justify-content: space-between;
                    gap: 16px;
                    padding: 16px 18px;
                    border: 1px solid var(--border);
                    border-radius: 14px;
                    background: rgba(255, 255, 255, 0.03);
                    color: var(--text);
                    text-decoration: none;
                    transition:
                        border-color 150ms ease,
                        transform 150ms ease,
                        background-color 150ms ease;
                }

                .link:hover,
                .link:focus-visible {
                    border-color: rgba(56, 189, 248, 0.55);
                    background: rgba(56, 189, 248, 0.08);
                    transform: translateY(-2px);
                    outline: none;
                }

                .link-title {
                    font-weight: 650;
                }

                .link-description {
                    color: var(--muted);
                    font-size: 0.875rem;
                }

                .arrow {
                    color: var(--accent);
                    font-weight: 700;
                }

                .status {
                    display: flex;
                    align-items: center;
                    gap: 9px;
                    margin-top: 28px;
                    color: var(--muted);
                    font-size: 0.8rem;
                }

                .dot {
                    width: 8px;
                    height: 8px;
                    border-radius: 999px;
                    background: var(--success);
                    box-shadow: 0 0 0 4px rgba(52, 211, 153, 0.15);
                }

                footer {
                    margin-top: 18px;
                    text-align: center;
                    color: var(--muted);
                    font-size: 0.8rem;
                }

                @media (max-width: 480px) {
                    .card {
                        padding: 26px;
                        border-radius: 16px;
                    }

                    .link {
                        align-items: flex-start;
                        flex-direction: column;
                    }

                    .arrow {
                        display: none;
                    }
                }
            </style>
        </head>
        <body>
            <main>
                <section class="card">
                    <div class="brand">
                        <div class="logo" aria-hidden="true">K</div>
                        <h1>Kursplassen API</h1>
                    </div>

                    <p class="subtitle">
                        Explore the interactive API documentation, inspect the OpenAPI specification,
                        or check that the service is running.
                    </p>

                    <nav class="links" aria-label="API resources">
                        <a class="link" href="/swagger">
                            <span>
                                <span class="link-title">Swagger UI</span><br />
                                <span class="link-description">Browse and test the available endpoints.</span>
                            </span>
                            <span class="arrow" aria-hidden="true">&rarr;</span>
                        </a>

                        <a class="link" href="/openapi/v1.json">
                            <span>
                                <span class="link-title">OpenAPI JSON</span><br />
                                <span class="link-description">Download the machine-readable API specification.</span>
                            </span>
                            <span class="arrow" aria-hidden="true">&rarr;</span>
                        </a>
                        
                        <a class="link" href="/scalar/v1">
                          <span>
                              <span class="link-title">Scalar</span><br />
                              <span class="link-description">Using scalar for api interactiosn</span>
                          </span>
                          <span class="arrow" aria-hidden="true">&rarr;</span>
                      </a>

                        <a class="link" href="/health">
                            <span>
                                <span class="link-title">Health check</span><br />
                                <span class="link-description">Verify the current service status.</span>
                            </span>
                            <span class="arrow" aria-hidden="true">&rarr;</span>
                        </a>
                    </nav>

                    <div class="status">
                        <span class="dot" aria-hidden="true"></span>
                        API documentation available
                    </div>
                </section>
            </main>

            <footer>
                Kursplassen API
            </footer>
        </body>
        </html>
        """;

    public static IEndpointRouteBuilder MapRootEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => Results.Content(
                LandingPageHtml,
                "text/html; charset=utf-8"))
            .ExcludeFromDescription();

        return app;
    }
}