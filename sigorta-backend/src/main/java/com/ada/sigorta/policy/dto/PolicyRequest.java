package com.ada.sigorta.policy.dto;



import com.ada.sigorta.policy.model.PolicyType;
import lombok.Data;

import java.time.LocalDate;

@Data
public class PolicyRequest {
    private PolicyType type;
    private LocalDate startDate;
    private LocalDate endDate;
    private double premium;
}
