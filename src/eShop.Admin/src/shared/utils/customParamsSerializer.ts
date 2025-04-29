export const customParamsSerializer = (params: Record<string, any>): string => {
    const searchParams = new URLSearchParams();
    Object.entries(params).forEach(([key, value]) => {
        if (Array.isArray(value)) {
            value.forEach(item => searchParams.append(key, item));
        } else {
            searchParams.append(key, value);
        }
    });

    return searchParams.toString();
}