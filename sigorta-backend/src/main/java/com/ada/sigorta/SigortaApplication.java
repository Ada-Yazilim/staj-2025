package com.ada.sigorta;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;

@SpringBootApplication
@ComponentScan(basePackages = {"com.ada.sigorta"})
@EnableJpaRepositories(basePackages= {"com.ada.sigorta"})
public class SigortaApplication {

	public static void main(String[] args) {
		SpringApplication.run(SigortaApplication.class, args);
	}

}
