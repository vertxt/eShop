export function cleanParams(params: Record<string, any>): Record<string, any> {
  const query: Record<string, any> = {};
  for (const key in params) {
    const value = params[key];
    if (
      value !== undefined &&
      value !== null &&
      !(typeof value === "string" && value.trim() === '') &&
      !(Array.isArray(value) && value.length === 0)
    ) {
      query[key] = Array.isArray(value) ? value : String(value);
    }
  }

  return query;
}
