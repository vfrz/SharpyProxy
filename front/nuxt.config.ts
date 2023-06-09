// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
    app: {
        head: {
            title: "SharpyProxy",
            htmlAttrs: {
                lang: "en",
            },
            bodyAttrs: {
                class: "bg-neutral-100"
            }
        }
    },
    router: {
        options: {
            linkExactActiveClass: "active"
        }
    },
    runtimeConfig: {
        public: {
            apiBaseUrl: "http://localhost:8181"
        }
    },
    appConfig: {
        version: "0.1.0"
    },
    css: [
        "@/assets/css/sharpy-proxy.css",
        "boxicons/css/boxicons.css"
    ],
    nitro: {
        compressPublicAssets: {
            brotli: true,
            gzip: true
        }
    },
    modules: [
        "@nuxtjs/tailwindcss",
        "nuxt-headlessui",
        "@vee-validate/nuxt"
    ],
    vite: {
        vue: {
            script: {
                defineModel: true
            }
        }
    }
})
