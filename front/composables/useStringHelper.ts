export default function () {

    const isNullOrWhitespace = (value: string): boolean => {
        return !value || !value.trim();
    }

    return {
        isNullOrWhitespace
    }
}