// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
    app: {
        head: {
            title: "SharpyProxy",
            htmlAttrs: {
                lang: 'en',
            },
            bodyAttrs: {
                class: 'bg-neutral-100 dark:bg-slate-900 dark:text-white'
            }
        }
    },
    router: {
        options: {
            linkExactActiveClass: 'active'
        }
    },
    runtimeConfig: {
        public: {
            apiBaseUrl: 'http://localhost:8080/.proxy-api'
        }
    },
    css: [
        '@/assets/css/sharpy-proxy.css',
        'boxicons/css/boxicons.css'
    ],
    nitro: {
        compressPublicAssets: {
            brotli: true,
            gzip: true
        }
    },
    modules: [
        '@nuxtjs/tailwindcss',
        'nuxt-headlessui'
    ]
})
