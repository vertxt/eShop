export function cleanParams(params: Record<string, any>): Record<string, string> {
  const query: Record<string, string> = {};
  for (const key in params) {
    const value = params[key];
    if (
      value !== undefined &&
      value !== null &&
      !(typeof value === "string" && value.trim() === '') &&
      !(Array.isArray(value) && value.length === 0)
    ) {
      query[key] = String(value);
    }
  }

  return query;
}
