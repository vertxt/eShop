import { Alert, AlertTitle, Box, Button, ButtonGroup, Stack, Typography } from "@mui/material";
import { useLazyGetBadRequestQuery, useLazyGetNotFoundQuery, useLazyGetServerErrorQuery, useLazyGetUnauthorizedQuery, useLazyGetValidationErrorQuery } from "./errorsApi";

export default function ErrorsControl() {
    const [trigger400] = useLazyGetBadRequestQuery();
    const [trigger401] = useLazyGetUnauthorizedQuery();
    const [trigger404] = useLazyGetNotFoundQuery();
    const [trigger500] = useLazyGetServerErrorQuery();
    const [triggerValidation, { error, isError }] = useLazyGetValidationErrorQuery();

    return (
        <Box display='flex' flexDirection='column' alignItems='center'>
            <Typography variant="h3" gutterBottom>Errors</Typography>
            <ButtonGroup>
                <Button onClick={() => trigger400()}>BadRequest</Button>
                <Button onClick={() => trigger401()}>Unauthorized</Button>
                <Button onClick={() => trigger404()}>NotFound</Button>
                <Button onClick={() => trigger500()}>Server Error</Button>
                <Button onClick={() => triggerValidation()}>Validation Error</Button>
            </ButtonGroup>
            {isError && 'fieldErrors' in error && error.fieldErrors && (
                <Stack sx={{ width: '100%', mt: '1rem' }} spacing={1}>
                    {Object.entries(error.fieldErrors).map(
                        ([field, messages], index) => (
                            <Alert key={index} severity="error">
                                <AlertTitle>{field}</AlertTitle>
                                {messages}
                            </Alert>
                        )
                    )}
                </Stack>
            )}
        </Box>
    )
}